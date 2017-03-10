using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Eventual.DAL;
using System.Data.Objects;
using System.Security.Cryptography;
using System.Text;

namespace Eventual_WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly EventFinderDB_DEVEntities db = new EventFinderDB_DEVEntities();
        private enum _typeOfEvent { SAVED, CURRENT, PAST };

        // GET: api/Users --> add http response
        public HttpResponseMessage GetUsers()
        {
            var temp = db.Users.ToList();

            List<Eventual.Model.User> users = new List<Eventual.Model.User>();

            //casts each user as an instance of an Eventual.Model.User
            foreach (var item in temp)
            {
                users.Add(ConvertModels.ConvertEntityToModel.UserEntityToUserModel(item));
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Detached;
            return Ok(user);
        }

        //todo implement hashing
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserID)
            {
                return BadRequest();
            }

            try
            {
                if (user != null && user.UserHashedPassword != null)
                {           
                    user.UserHashedPassword = ComputeHash(user.UserHashedPassword, new SHA256CryptoServiceProvider(),
                        Encoding.ASCII.GetBytes(GetDBSALT()));
                }

                db.spUpdateUser(user.UserFirstName, user.UserLastName, user.UserEmail, user.UserBirthDate,
                user.UserPhoneNumber, user.UserHashedPassword, user.UserImageURL, user.UserID);

                await db.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                return BadRequest(sqlEx.Message);
            }

            return Ok(user);
        }

        //todo implement hashing
        // POST: api/Users
        [ResponseType(typeof(Eventual.Model.User))]
        public async Task<IHttpActionResult> PostUser(Eventual.Model.User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.UserHashedPassword = ComputeHash(user.UserHashedPassword, new SHA256CryptoServiceProvider(), 
                Encoding.ASCII.GetBytes(GetDBSALT()));
            Eventual.DAL.User DALUser = ConvertModels.ConvertModelToEntity.UserModelToUserEntity(user);
            db.spCreateUser(DALUser.UserEmail, DALUser.UserHashedPassword);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = DALUser.UserID }, DALUser);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            DropRegisteredEvents(id);

            await db.SaveChangesAsync();

            DropSavedEvents(id);
            await db.SaveChangesAsync();
                      
            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserID == id) > 0;
        }

        //Drops all of users saved events
        private void DropSavedEvents(int id)
        {
            //if the user does not exist then don't even bother
            if (!UserExists(id))
            {
                return;
            }

            var savedEvents = db.spGetAllSavedEventsForSpecificUser(id).ToList();
 
            foreach (var item in savedEvents)
            {
                db.spDropSavedEventWithUserID(id, item.EventID);
            }
        }

        private void DropRegisteredEvents(int id)
        {
            //if the user does not exist then don't even bother
            if (!UserExists(id))
            {
                return;
            }

            var registeredEvents = db.spGetAllCurrentRegisteredEventsForSpecificUser(id).ToList();
            var pastEvents       = db.spGetAllPastRegisteredEventsForSpecificUser(id).ToList();

            foreach (var item in registeredEvents)
            {
                db.spDropRegisteredEventWithUserId(id, item.EventID);
            }

            foreach (var item in pastEvents)
            {
                db.spDropRegisteredEventWithUserId(id, item.EventID);
            }
        }

        //GET: api/Users/5
        [HttpGet]
        [ActionName("GetUsersEvents")]
        [ResponseType(typeof(Dictionary<_typeOfEvent, List<Eventual.Model.EventAPI>>))]
        public async Task<IHttpActionResult> GetUsersEvents(int id)
        {
            User user = await db.Users.FindAsync(id);

            //return a bad request response
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            //current, past, and present events
            List<Eventual.Model.EventAPI> current = 
                ConvertModels.ConvertEntityToModel.EventAPIEntityToEventAPIModel(db.spGetAllCurrentRegisteredEventsForSpecificUser(id).ToList());
            List<Eventual.Model.EventAPI> past =
                ConvertModels.ConvertEntityToModel.EventAPIEntityToEventAPIModel(db.spGetAllPastRegisteredEventsForSpecificUser(id).ToList());
            List<Eventual.Model.EventAPI> saved =
                ConvertModels.ConvertEntityToModel.EventAPIEntityToEventAPIModel(db.spGetAllSavedEventsForSpecificUser(id).ToList());

            //creates a new dictionary of EventAPI's
            Dictionary<_typeOfEvent, List<Eventual.Model.EventAPI>> userEvents = new Dictionary<_typeOfEvent, List<Eventual.Model.EventAPI>>();

            //stores them in a dictionary
            userEvents.Add(_typeOfEvent.CURRENT, current);
            userEvents.Add(_typeOfEvent.PAST, past);
            userEvents.Add(_typeOfEvent.SAVED, saved);

            //returns an ok with status code
            return Ok(userEvents);
        }

        private string GetDBSALT()
        {
            return db.spGetSALT().FirstOrDefault();
        }

        private string ComputeHash(string input, HashAlgorithm algorithm, byte[] salt)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] saltedInput = new byte[salt.Length + inputBytes.Length];
            salt.CopyTo(saltedInput, 0);
            inputBytes.CopyTo(saltedInput, salt.Length);
            byte[] hashedBytes = algorithm.ComputeHash(saltedInput);
            return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
        }
    }
}
