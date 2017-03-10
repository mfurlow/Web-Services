using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationStreet1 { get; set; }
        public string LocationStreet2 { get; set; }
        public string LocationZipcode { get; set; }
        public string LocationCity { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        
        public string StateAbbreviation { get; set; }
        public string CountryAbbreviation { get; set; }
        public string LocationBuildingName { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual State State { get; set; }
    }
}
