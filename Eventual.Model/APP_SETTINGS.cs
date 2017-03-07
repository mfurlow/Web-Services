using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    class APP_SETTINGS
    {
        public string SALT { get; set; }
        public int MAX_EVENTS_PER_USER { get; set; }
    }
}
