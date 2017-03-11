using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eventual.Model;
using System.Collections.ObjectModel;
using System.Security.Principal;

namespace Eventual_WebAPI.Utility
{
    public static class CurrentlyLoggedInUser
    {
            private static GenericPrincipal genericPrincipal;
            private static CustomIdentity customIdentity;


            public static void SetCurrentUser(string token, string secret, string[] thisUsersRoles)
            {
                IDictionary<string, object> decodedToken = new JsonWebToken(secret).Decode(token);
                var userId = 0;

                string userEmail = decodedToken.ContainsKey("userEmail") ? decodedToken["userEmail"].ToString() : string.Empty;

                if (decodedToken.ContainsKey("userID"))
                {

                    int.TryParse(decodedToken["userID"].ToString(), out userId);
                }

                customIdentity = new CustomIdentity(userEmail, userId) { BootstrapContext = token };

                genericPrincipal = new GenericPrincipal(customIdentity, thisUsersRoles);

                //stores custom identity
                //System.Threading.Thread.CurrentPrincipal = genericPrincipal;
            }

            public static int UserId => customIdentity?.UserId ?? -1;

            public static string Username => customIdentity == null ? "Anonymous" : customIdentity.Name;

            public static bool IsInRole(string role)
            {
                return genericPrincipal != null && genericPrincipal.IsInRole(role);
            }
        }

    }