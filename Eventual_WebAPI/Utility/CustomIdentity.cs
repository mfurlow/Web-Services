using System.Security.Principal;

namespace Eventual_WebAPI.Utility
{
    public class CustomIdentity : GenericIdentity
    {
            public CustomIdentity(string userEmail, int userId) : base(userEmail)
            {
                UserId = userId;
            }

            public int UserId { get; }
    }
}
