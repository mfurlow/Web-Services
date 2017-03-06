using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class EventType
    {
        public EventType()
        {
            this.Events = new HashSet<Event>();
        }

        public int EventTypeID { get; set; }
        public string EventTypeName { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
