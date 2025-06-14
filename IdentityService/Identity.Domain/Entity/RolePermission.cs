using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{

    public class RolePermission
    {
        public RolePermission()
        {
            
        }
        public long Id { get; set; }
        public long RoleID { get; set; }
        public long PermissionID { get; set; } 
        

    }
}
