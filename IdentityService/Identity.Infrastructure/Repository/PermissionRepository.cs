using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repository
{
    public class PermissionRepository 
    {
        private readonly BaseDbContext dbContext;

        public PermissionRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Delete(Permission info)
        {
            dbContext.Permissions.Remove(info);
        }

        public async Task AddAsync(Permission info)
        {
            await dbContext.Permissions.AddAsync(info);

        }

        public async Task<List<Role>> GetRolesByPermissionId(long permissionId)
        {
            var permission = await dbContext.Permissions
                .Include(t => t.Roles)
                .FirstAsync(t => t.Id == permissionId);

            return permission.Roles.ToList();
        }

        public async Task<Permission?> GetByPermissionKeyAsync(string key,string systemName)
        {
            var res = await dbContext.Permissions.FirstOrDefaultAsync(t => t.PermissionKey.ToLower().Trim() == key.ToLower().Trim() && t.SystemName.ToLower().Trim() == systemName.ToLower().Trim());
            return res;
        }
        public async Task<List<Permission>> GetPermissionsByIds(List<long> ids)
        {
            var res = await dbContext.Permissions.Where(t => ids.Contains(t.Id)).ToListAsync();
            return res;
        }
        public async Task<Permission?> GetByIdAsync(long id)
        {
            var res = await dbContext.Permissions.FirstOrDefaultAsync(t => t.Id == id);
            return res;
        }

    }
}
