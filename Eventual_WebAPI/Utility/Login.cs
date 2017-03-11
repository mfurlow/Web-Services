using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Eventual_WebAPI.Utility
{
    public static class Login
    {
        public static Eventual.Model.User LoginValidator(Controllers.LoginController login, Eventual.Model.LoginCredentials loginCredential)
        {
            HttpResponseMessage response = login.Login(loginCredential);

            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadAsAsync<Eventual.Model.User>().Result;

                return user;
            }

            return null;
        }
    }
}