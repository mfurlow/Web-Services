using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eventual.Model;
using System.Collections.ObjectModel;

namespace Eventual_WebAPI.Utility
{
    public static class CurrentlyLoggedInUser
    {
        public static JsonWebToken AccessToken { get; set; }
        public static int UserID { get; set; }
        public static string UserEmail { get; set; }
        public static string UserFirstName { get; set; }
        public static string UserLastName { get; set; }
        public static Nullable<System.DateTime> UserStartDate { get; set; }
        public static Nullable<System.DateTime> UserBirthDate { get; set; }
        public static Nullable<System.DateTime> UserEndDate { get; set; }
        public static string UserPhoneNumber { get; set; }
        public static Nullable<int> UserRoleID { get; set; }
        public static string UserHashedPassword { get; set; }
        public static string UserImageURL { get; set; }

        public static ICollection<EventRegistration> EventRegistrations { get; set; }
        public static Collection<SavedEvent> SavedEvents { get; set; }
        public static UserRole UserRole { get; set; }

    }
}