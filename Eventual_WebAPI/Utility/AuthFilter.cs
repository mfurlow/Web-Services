using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Eventual_WebAPI.Utility
{
    public class AuthFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                //jwt stored in the parameter
                AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;
                if (authorization?.Scheme != "Bearer" || string.IsNullOrEmpty(authorization?.Parameter))
                {
                    return false;
                }

                CurrentlyLoggedInUser.SetCurrentUser(authorization.Parameter, "fuckthisshit", new[] { "User" });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}