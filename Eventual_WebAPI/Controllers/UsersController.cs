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

namespace Eventual_WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private EventFinderDB_DEVEntities db = new EventFinderDB_DEVEntities();

        // GET: api/Users
        public List<Eventual.Model.User> GetUsers()
        {
            var temp = db.Users.ToList();

            List<Eventual.Model.User> users = new List<Eventual.Model.User>();

            //casts each user as an instance of an Eventual.Model.User
            foreach (var item in temp)
            {
                users.Add(ConvertModels.ConvertEntityToModel.UserEntityToUserModel(item));
            }

            return users;
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

            return Ok(user);
        }

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

            db.Entry(user).State = EntityState.Modified;

            try
            {
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(Eventual.Model.User))]
        public async Task<IHttpActionResult> PostUser(Eventual.Model.User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Eventual.DAL.User DALUser = ConvertModels.ConvertModelToEntity.UserModelToUserEntity(user);

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
    }
}