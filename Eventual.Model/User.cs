using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class User
    {
        public User()
        {
            this.Events = new HashSet<Event>(); //Events that users can organize
            this.EventRegistrations = new HashSet<EventRegistration>();
            this.SavedEvents = new HashSet<SavedEvent>();
        }

        public int UserID { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public Nullable<System.DateTime> UserStartDate { get; set; }
        public Nullable<System.DateTime> UserBirthDate { get; set; }
        public Nullable<System.DateTime> UserEndDate { get; set; }
        public string UserPhoneNumber { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public string UserHashedPassword { get; set; }
        public string UserImageURL { get; set; }

        //Events that users can organize
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; }
        public virtual ICollection<SavedEvent> SavedEvents { get; set; }
        public virtual UserRole UserRole { get; set; }

    }
}
