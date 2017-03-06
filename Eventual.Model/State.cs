using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class State
    {
        public State()
        {
            this.Locations = new HashSet<Location>();
        }

        public int StateID { get; set; }
        public string StateAbbreviation { get; set; }
        public string StateLongName { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
