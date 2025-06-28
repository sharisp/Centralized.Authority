
using Identity.Domain.Events;
using Identity.Domain.Interfaces;
using System.Collections.Generic;

namespace Identity.Domain.Entity
{
    public class Role :  BaseAuditableEntity, IAggregateRoot
    {
        private Role()
        {

        }

        public Role(string roleName,string? description = null)
        {
            RoleName = roleName;
            this.CreateTime = DateTime.Now;
            this.Description = description;
            AddDomainEvent(new RoleAddEvents(this));
        }

        public string RoleName { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreateTime { get;private set; } 
        public List<User> Users { get; private set; } = new List<User>(); // 角色对应的用户列表

        public List<Permission> Permissions { get; private set; }=new List<Permission>();

        public List<Menu> Menus { get; private set; } = new List<Menu>(); 
        public void AddPermissions(List<Permission> permissions)
        {
            
            Permissions.AddRange(permissions);
           // AddDomainEvent(new RoleAddEvents(this));
           AddDomainEvent(new RoleAssignEvents(this,permissions));
        }
        public void AddMenus(List<Menu> menus)
        {

            menus.AddRange(menus);
            // AddDomainEvent(new RoleAddEvents(this));
           // AddDomainEvent(new RoleAssignEvents(this, menus));
        }
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
