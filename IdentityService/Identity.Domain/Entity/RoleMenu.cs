using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{

    public class RoleMenu
    {
        public RoleMenu()
        {
            
        }
        public long Id { get; set; }


        public long RoleId { get; set; }

        public long MenuId { get; set; }

    }
}
