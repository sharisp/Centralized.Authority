using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interfaces
{
    public interface IRoleRepository
    {
        public void AddRole(string roleName, string? description);
        public void UpdateRole(long roleId, string roleName, string? description);
        public void DeleteRole(long roleId);
        public List<long> GetRoleIdsByUserId(long userId);
      
    }
}
