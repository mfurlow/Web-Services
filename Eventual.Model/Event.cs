using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class Event
    { 
        //public Event()
        //{
        //    this.EventRegistrations = new HashSet<EventRegistration>();
        //    this.SavedEvents = new HashSet<SavedEvent>();
        //    this.EventTypes = new HashSet<EventType>();
        //}

        public int EventID { get; set; }
        public System.DateTime EventStartTime { get; set; }
        public System.DateTime EventEndTime { get; set; }
        public string EventTitle { get; set; }
        public decimal EventPrice { get; set; }
        public string EventDescription { get; set; }
        public int LocationID { get; set; }
        public string EventImageURL { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<EventRegistration> EventRegistrations { get; set; }

        public virtual ICollection<SavedEvent> SavedEvents { get; set; }

        public virtual ICollection<EventType> EventTypes { get; set; }
    }
}
