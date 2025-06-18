
using Identity.Domain.Events;
using Identity.Domain.Interfaces;

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

        public string RoleName { get; set; }
        public string? Description { get; set; }
        public DateTime CreateTime { get; set; } 
        public List<User> Users { get; set; } = new List<User>(); // 角色对应的用户列表

        public List<Permission> Permissions { get; set; }=new List<Permission>(); // 角色对应的权限列表)

        public void AddPermissions(List<Permission> permissions)
        {
            
            Permissions.AddRange(permissions);
           // AddDomainEvent(new RoleAddEvents(this));
           AddDomainEvent(new RoleAssignEvents(this,permissions));
        }
        public void ChangeRoleName(string roleName)
        {
          
            RoleName = roleName;
          //  AddDomainEvent(new RoleChangeEvents(this));
        }
        public void ChangeDescription(string? description)
        {

            Description = description;
         //   AddDomainEvent(new RoleChangeEvents(this));
        }
    }
}
