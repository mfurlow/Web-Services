using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventual.Model
{
    public class UserRole
    {
        public UserRole()
        {
            //stores all of the users that associated with that role
            this.Users = new HashSet<User>();
        }
        public int UserRoleID { get; set; }
        public string UserRoleType { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
