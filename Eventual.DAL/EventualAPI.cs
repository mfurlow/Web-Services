using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.DAL
{
    public class EventualAPI
    {
        public int EventID { get; set; }
        public System.DateTime EventStartTime { get; set; }
        public System.DateTime EventEndTime { get; set; }
        public string EventTitle { get; set; }
        public decimal EventPrice { get; set; }
        public string LocationStreet1 { get; set; }
        public string LocationCity { get; set; }
        public string StateAbbreviation { get; set; }
        public string EventImageURL { get; set; }
        public int UserID { get; set; }

    }
}
