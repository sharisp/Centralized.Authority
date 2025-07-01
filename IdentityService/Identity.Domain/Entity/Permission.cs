using Identity.Domain.Interfaces;

namespace Identity.Domain.Entity
{

    public class Permission : BaseAuditableEntity, IAggregateRoot
    {
        private Permission() { }
        public string Title { get; private set; }
        public string SystemName { get; private set; }//for multi-system support, e.g., "Identity", "Order", etc.
        public string PermissionKey { get;private set; }//user:add 

       // public long? MenuID { get; set; }//对应的menu，可选  拆分一个表，多对多关系
        public string? ApiPath { get; private set; }//api路径
        public string? HttpMethod { get; private set; }//get,post,put,delete,patch,options

        public string? Controller { get; private set; }
        public string? Action { get; private set; }

        public List<Role> Roles { get; private set; }= new List<Role>(); // 权限对应的角色列表


        public List<Menu> Menus { get; private set; } = new List<Menu>();

        public Permission(string title, string systemName, string permissionKey, string? apiPath = null, string? httpMethod = null, string? controller = null, string? action = null)
        {
            Title = title;
            SystemName = systemName;
            PermissionKey = permissionKey;
            ApiPath = apiPath;
            HttpMethod = httpMethod;
            Controller = controller;
            Action = action;
        }

        public void ChangeTitle(string title)
        {
            Title = title;
        }
       public void ChangeSystemName(string systemName)
        {
            SystemName = systemName;
        }
        public void ChangePermissionKey(string permissionKey)
        {
            PermissionKey = permissionKey;
        }
    }
}
