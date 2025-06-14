
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class Role 
    {
        private Role()
        {

        }

        public Role(string roleName,string? description)
        {
            RoleName = roleName;
            this.CreateTime = DateTime.Now;
            this.Description = description;
            this.RoleId = IdGeneratorFactory.NewId();
        }

        public long RoleId { get;private set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }
        public DateTime CreateTime { get; set; } 
    }
}
