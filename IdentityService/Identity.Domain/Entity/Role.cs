
using Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Events;

namespace Identity.Domain.Entity
{
    public class Role :  BaseAuditableEntity, IAggregateRoot
    {
        private Role()
        {

        }

        public Role(string roleName,string? description)
        {
            RoleName = roleName;
            this.CreateTime = DateTime.Now;
            this.Description = description;
            AddDomainEvent(new RoleAddEvents(this));
        }

        public string RoleName { get; set; }
        public string? Description { get; set; }
        public DateTime CreateTime { get; set; } 
        public List<User> Users { get; set; } = new List<User>(); // 角色对应的用户列表

        public void ChangeRoleName(string roleName)
        {
          
            RoleName = roleName;
            AddDomainEvent(new RoleChangeEvents(this));
        }
        public void ChangeDescription(string? description)
        {

            Description = description;
            AddDomainEvent(new RoleChangeEvents(this));
        }
    }
}
