using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class SavedEvent
    {
        public int UserID { get; set; }
        public int EventID { get; set; }
        public Nullable<System.DateTime> DateOfSavingEvent { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
