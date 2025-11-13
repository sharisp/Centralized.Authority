using Identity.Domain.Entity;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Services
{
    public class RoleDomainService(ISystemConfigRepository systemConfigRepository, IRoleRepository roleRepository)
    {
        public async Task<List<Role>?> GetDefaultRolesAsync()
        {
            List<Role>? roles = null;
            var roleConfig = await systemConfigRepository.GetByConfigKey("DefaultUserRoles", null);
            if (roleConfig != null)
            {
                var defaultRoleIds = roleConfig.ConfigValue?.Split(',');
                if (defaultRoleIds != null && defaultRoleIds.Length > 0)
                {
                    var roleIdLong = defaultRoleIds.Select(t => Convert.ToInt64(t)).ToList();
                    roles = await roleRepository.GetRolesByIds(roleIdLong);
                }
            }
            return roles;
        }

    }
}
