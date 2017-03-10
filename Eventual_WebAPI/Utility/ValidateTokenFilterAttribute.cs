using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Eventual_WebAPI.Utility
{
    //see: http://www.c-sharpcorner.com/UploadFile/db2972/trace-web-api-execution-time-using-custom-action-filter/
    //see: https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters

    /**
     * These AuthenticateAsync and ChallengeAsync methods correspond to the authentication flow defined in RFC 2612 and RFC 2617:
     * The client sends credentials in the Authorization header. This typically happens after the client receives a 401 (Unauthorized) 
     * response from the server. However, a client can send credentials with any request, not just after getting a 401.
     * If the server does not accept the credentials, it returns a 401 (Unauthorized) response. The response includes 
     * a Www-Authenticate header that contains one or more challenges. Each challenge specifies an authentication scheme recognized by the server.
     * */
    public class ValidateTokenFilterAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        //validates token in the request
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                return;
            }

            //if (String.IsNullOrEmpty(authorization.Parameter))
            //{
            //    context.ErrorResult = new AuthenticationFailureResult("Missing Credentials");
            //    return;
            //}

            //Tuple<string, string> userNameAndPassword = ExtractUserNameAndPassword()
        }

        //adds authentication challenge to HTTP response, if needed
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}