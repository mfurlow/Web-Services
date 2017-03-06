using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class Country
    {
        public Country()
        {
            this.Locations = new HashSet<Location>();
        }
        public int CountryID { get; set; }
        public string CountryAbbreviation { get; set; }
        public string CountryLongName { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
