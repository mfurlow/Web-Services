using System;
using System.Collections.Generic;
using System.Text;
using Jose;
using Newtonsoft.Json;

namespace Eventual_WebAPI.Utility
{           
    // see https://auth0.com/docs/tutorials/generate-jwt-dotnet for tutorial
    public class JsonWebToken
    {
        private readonly string secret;
        private readonly JwsAlgorithm algorithm;

        public JsonWebToken(string secret) : this(secret, JwsAlgorithm.HS256) { }

        public JsonWebToken(string secret, JwsAlgorithm algorithm)
        {
            if (algorithm == JwsAlgorithm.HS256 || algorithm == JwsAlgorithm.HS384 || algorithm == JwsAlgorithm.HS512)
            {
                this.secret = secret;
                this.algorithm = algorithm;
            }
            else
            {
                throw new Exception("Only HS256, HS384, and HS512 hashing algorithms are allowed");
            }
        }

        public static bool IsTokenExpired(string token)
        {

            string payloadBytes = token.Split('.')[1];

            int mod4 = payloadBytes.Length % 4;
            if (mod4 > 0) payloadBytes += new string('=', 4 - mod4);

            byte[] payloadBytesDecoded = Convert.FromBase64String(payloadBytes);

            string payloadStr = Encoding.UTF8.GetString(payloadBytesDecoded, 0, payloadBytesDecoded.Length);
            var payload = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL });

            var currentTimestamp = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            return currentTimestamp > (payload.Exp - 60);
        }

        public bool Validate(string token)
        {
            try
            {
                JWT.Decode<IDictionary<string, object>>(token, Encoding.UTF8.GetBytes(secret), algorithm);
                return true;

            }
            catch (IntegrityException)
            {
                return false;
            }
        }

        public IDictionary<string, object> Decode(string token)
        {
            return JWT.Decode<IDictionary<string, object>>(token, Encoding.UTF8.GetBytes(secret), algorithm);
        }

        public string GenerateToken(Dictionary<string, object> claims, Dictionary<string, object> headers = null)
        {
            if (headers != null)
            {
                if (!headers.ContainsKey("typ"))
                {
                    // this field must ALWAYS be set in the header
                    headers.Add("typ", "JWT");
                }
            }

            return JWT.Encode(claims, Encoding.UTF8.GetBytes(secret), algorithm, headers);
        }

        public T DecodeToObject<T>(string token)
        {
            return JWT.Decode<T>(token, Encoding.UTF8.GetBytes(secret), algorithm);
        }
    }
}