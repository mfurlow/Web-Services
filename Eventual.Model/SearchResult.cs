using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class SearchResult
    {
        public int EventID { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public string EventTitle { get; set; }
        public decimal EventPrice { get; set; }
        
        public string EventImageURL { get; set; }
        public string LocationBuildingName { get; set; }
        
        public string LocationStreet1 { get; set; }
        public string LocationCity { get; set; }
        public string StateAbbreviation { get; set; }

    }
}
