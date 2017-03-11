using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Eventual.DAL;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Eventual.Model;

namespace Eventual_WebAPI.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly EventFinderDB_DEVEntities db = new EventFinderDB_DEVEntities();
        private enum _typeOfEvent { SAVED, CURRENT, PAST };

        private LoginController loginController = new LoginController()
        {
            Request = new HttpRequestMessage(),
            Configuration = new HttpConfiguration()
        };


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


        private Eventual.Model.User ValidateUser(string userEmail, string password)
        {
            //regular password
            LoginCredentials login = new Eventual.Model.LoginCredentials
            {
                UserEmail = userEmail,
                UserPassword = password //regular unhashed password
            };

            return Utility.Login.LoginValidator(loginController, login);
        }

    
        // GET: api/Users/5
        [ResponseType(typeof(Eventual.Model.User))]
        [Route("GetUser/{id}/{userEmail}/{password}")]
        public async Task<IHttpActionResult> GetUser([FromUri]int id, [FromUri]string userEmail,
            [FromUri]string password)
        {
            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Unauthorized();
            }

            Eventual.DAL.User user = await db.Users.FindAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }
            
            if (user.UserID != u.UserID)
            {
                return Unauthorized();
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Detached;
            return Ok(user);
        }

        //todo implement hashing
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("PutUser/{id}/{userEmail}/{password}")]
        public async Task<IHttpActionResult> PutUser([FromUri]int id, [FromBody] Eventual.Model.User user,
            [FromUri]string userEmail, [FromUri]string password)
        {
            Eventual.Model.User updatedUser = null;

            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user != null && user.UserHashedPassword != null)
                {
                    user.UserHashedPassword = ComputeHash(user.UserHashedPassword, new SHA256CryptoServiceProvider(),
                        Encoding.ASCII.GetBytes(GetDBSALT()));
                }

                //todo --- update the currently logged in user
                updatedUser = ConvertModels.ConvertEntityToModel.UserEntityToUserModel(db.spUpdateUser(user.UserFirstName, user.UserLastName, user.UserEmail, user.UserBirthDate,
                user.UserPhoneNumber, user.UserHashedPassword, user.UserImageURL, id).FirstOrDefault());

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
            catch (SqlException sqlEx)
            {
                return BadRequest(sqlEx.Message);
            }

            return Ok(updatedUser);
        }

        // POST: api/Users
        [ResponseType(typeof(Eventual.Model.User))]
        [HttpPost]
        public async Task<IHttpActionResult> SignUpUser([FromBody]Eventual.Model.User user)
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
        [ResponseType(typeof(Eventual.Model.User))]
        public async Task<IHttpActionResult> DeleteUser([FromUri]int id)
        {
            
            Eventual.DAL.User user = await db.Users.FindAsync(id);

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

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventID == id) > 0;
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
        [Route("DropSavedEvent/{userID}/{savedEvent}/{userEmail}/{password}")]
        public HttpResponseMessage DropSavedEvent([FromUri]int userID, [FromUri]int savedEvent,
            [FromUri]string userEmail, [FromUri]string password)
        {
            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            
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
        [Route("DropRegisteredEvent/{userID}/{registeredEvent}/{userEmail}/{password}")]
        public HttpResponseMessage DropRegisteredEvent([FromUri]int userID, [FromUri]int registeredEvent,
            [FromUri]string userEmail, [FromUri]string password)
        {

            var temp = db.Users.ToList();

            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (!UserExists(userID) || (!CurrentEventExists(userID, registeredEvent) 
                && !PastEventExists(userID, registeredEvent)))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.spDropRegisteredEventWithUserId(userID, registeredEvent);

            return Request.CreateResponse(HttpStatusCode.OK,
                db.spGetAllCurrentRegisteredEventsForSpecificUser(userID));
        }
        

        private void DropRegisteredEvents(int id)
        {
            //if the user does not exist then don't even bother
            if (!UserExists(id))
            {
                return;
            }

            var registeredEvents = db.spGetAllCurrentRegisteredEventsForSpecificUser(id).ToList();
            var pastEvents = db.spGetAllPastRegisteredEventsForSpecificUser(id).ToList();

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
        [Route("GetUsersEvents/{id}/{userEmail}/{password}")]
        [ResponseType(typeof(Dictionary<_typeOfEvent, List<Eventual.Model.EventAPI>>))]
        public async Task<IHttpActionResult> GetUsersEvents([FromUri]int id, [FromUri]string userEmail,
            [FromUri]string password)
        {
            Eventual.DAL.User user = await db.Users.FindAsync(id);

            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Unauthorized();
            }

            //return a bad request response
            if (user == null)
            {
                return BadRequest(ModelState);
            }
            
            //returns an ok with status code
            return Ok(GetAllUsersEvents(id));
        }

        private Dictionary<_typeOfEvent, List<Eventual.Model.EventAPI>> GetAllUsersEvents(int id)
        {
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

            return userEvents;
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

        //registers users and returns all user events
        [HttpPost]
        [Route("RegisterEvent/{userID}/{eventID}/{userEmail}/{password}")]
        public HttpResponseMessage RegisterEvent([FromUri]int userID, [FromUri]int eventID,
            [FromUri]string userEmail, [FromUri]string password)
        {
            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (CurrentEventExists(userID, eventID) || PastEventExists(userID, eventID))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Already added event");
            }

            try
            {
                if (!UserExists(userID) || !EventExists(eventID))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request");
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (EntityException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            db.spJoinRegisteredEventUserId(userID, eventID);

            return Request.CreateResponse(HttpStatusCode.OK, GetAllUsersEvents(userID));
        }

        [HttpPost]
        [Route("SaveEvent/{userID}/{eventID}/{userEmail}/{password}")]
        public HttpResponseMessage SaveEvent(int userID, int eventID, string userEmail, string password)
        {
            Eventual.Model.User u = ValidateUser(userEmail, password);

            if (u == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            if (SavedEventExists(userID, eventID))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Already saved event");
            }

            try
            {
                if (!UserExists(userID) || !EventExists(eventID))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request");
                }
            }
            catch (SqlException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (EntityException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            db.spSaveEventUserId(userID, eventID);

            return Request.CreateResponse(HttpStatusCode.OK, GetAllUsersEvents(userID));
        }

    }
}
