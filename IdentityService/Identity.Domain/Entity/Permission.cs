using Identity.Domain.Interfaces;

namespace Identity.Domain.Entity
{

    public class Permission : BaseAuditableEntity, IAggregateRoot
    {

        public string Title { get; set; }
        public string SystemName { get; set; }//for multi-system support, e.g., "Identity", "Order", etc.
        public string PermissionKey { get; set; }//user:add 

       // public long? MenuID { get; set; }//对应的menu，可选  拆分一个表，多对多关系
        public string? ApiPath { get; set; }//api路径
        public string? HttpMethod { get; set; }//get,post,put,delete,patch,options

        public string? Controller { get; set; }
        public string? Action { get; set; }

        public List<Role> Roles { get; set; }= new List<Role>(); // 权限对应的角色列表
    }
}
