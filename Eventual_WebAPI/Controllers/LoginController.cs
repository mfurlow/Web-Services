using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Eventual.DAL;
using Eventual_WebAPI.Utility;

namespace Eventual_WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        private readonly EventFinderDB_DEVEntities db = new EventFinderDB_DEVEntities();
        //return salt
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

        private bool ArePasswordsEqual(string HashedAndSaltedPassword, string passwordAttempt)
        {
            return HashedAndSaltedPassword.Equals(ComputeHash(passwordAttempt, new SHA256CryptoServiceProvider(), 
                Encoding.ASCII.GetBytes(GetDBSALT())));
        }

        [HttpPost]
        public HttpResponseMessage Login(Eventual.Model.LoginCredentials login) 
        {
            if (string.IsNullOrEmpty(login.UserEmail) || string.IsNullOrEmpty(login.UserPassword))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username or password");
            }

            User user = db.Users.FirstOrDefault(u => u.UserEmail.Equals(login.UserEmail));
            
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username or password");
            }

            if (!ArePasswordsEqual(user.UserHashedPassword, login.UserPassword))
            {


                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            db.Entry(user).State = System.Data.Entity.EntityState.Detached;

            //Dictionary<string, object> headers = new Dictionary<string, object>();
            //todo to convert unix time to timestamp string and then expiration two hours from now
            //Dictionary<string, object> claims = new Dictionary<string, object>
            //{
            //    {"iss", "API.Eventual"},
            //    {"iat", ""},
            //    {"exp", ""},
            //    {"userID", user.UserID.ToString() },
            //    { "userEmail", user.UserEmail }
            //};

            //string secret = "fuckthisshit";
            //string jwt = new JsonWebToken(secret).GenerateToken(claims, headers);

            //returns validated user
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        
    }
}
