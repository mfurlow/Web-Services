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
    [RoutePrefix("api/users")]
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

        //TODO - unique key constraint for password
        // POST: api/Users
        [ResponseType(typeof(Eventual.Model.User))]
        public async Task<IHttpActionResult> SignUpUser(Eventual.Model.User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (db.Users.Count(u => u.UserEmail == user.UserEmail) > 0)
            {
                return BadRequest("Please login.");
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

        [HttpDelete]
        [Route("DropSavedEvent/{userID}/{savedEvent}")]
        public HttpResponseMessage DropSavedEvent([FromUri]int userID, [FromUri]int savedEvent)
        {
            var temp = db.Users.ToList();

            if (!UserExists(userID) || !SavedEventExists(userID, savedEvent))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.spDropSavedEventWithUserID(userID, savedEvent);

            return Request.CreateResponse(HttpStatusCode.OK, 
                db.spGetAllSavedEventsForSpecificUser(userID));
        }

        [HttpDelete]
        [Route("DropCurrentEvent/{userID}/{currentEvent}")]
        public HttpResponseMessage DropCurrentEvent([FromUri]int userID, [FromUri]int currentEvent)
        {

            var temp = db.Users.ToList();

            if (!UserExists(userID) || !CurrentEventExists(userID, currentEvent))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.spDropRegisteredEventWithUserId(userID, currentEvent);

            return Request.CreateResponse(HttpStatusCode.OK,
                db.spGetAllCurrentRegisteredEventsForSpecificUser(userID));
        }

        [HttpDelete]
        [Route("DropPastEvent/{userID}/{pastEvent}")]
        public HttpResponseMessage DropPastEvent([FromUri]int userID, [FromUri]int pastEvent)
        {

            var temp = db.Users.ToList();

            if (!UserExists(userID) || !PastEventExists(userID, pastEvent))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.spDropRegisteredEventWithUserId(userID, pastEvent);

            return Request.CreateResponse(HttpStatusCode.OK,
                db.spGetAllPastRegisteredEventsForSpecificUser(userID));
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

        private bool SavedEventExists(int userID, int savedEventID)
        {
            return (db.spGetAllSavedEventsForSpecificUser(userID).ToList().Count(se => se.EventID == savedEventID) > 0);
        }

        private bool PastEventExists(int userID, int pastEventID)
        {
            return (db.spGetAllPastRegisteredEventsForSpecificUser(userID).ToList().Count(se => se.EventID == pastEventID) > 0);
        }

        private bool CurrentEventExists(int userID, int currentEventID)
        {
            return (db.spGetAllCurrentRegisteredEventsForSpecificUser(userID).ToList().Count(se => se.EventID == currentEventID) > 0);
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
