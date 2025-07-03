using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;
using IdGen;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repository
{
    public class RoleRepository 
    {
        private readonly BaseDbContext dbContext;

        public RoleRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void DeleteRole(Role role)
        {
            dbContext.Roles.Remove(role);
        }

        public async Task AddAsync(Role role)
        {
            await dbContext.Roles.AddAsync(role);

        }

        public async Task<List<User>> GetUsersByRoleId(long roleId)
        {
            var role = await dbContext.Roles
                .Include(t => t.Users)
                .FirstAsync(t => t.Id == roleId);

            return role.Users.ToList();
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            var res = await dbContext.Roles.FirstOrDefaultAsync(t => t.RoleName == name);
            return res;
        }
        public async Task<List<Role>> GetRolesByIds(List<long> ids)
        {
            var res = await dbContext.Roles.Where(t => ids.Contains(t.Id)).ToListAsync();
            return res;
        }
        public async Task<Role?> GetByIdAsync(long id)
        {
            var res = await dbContext.Roles.FirstOrDefaultAsync(t => t.Id == id);
            return res;
        }

     

    }
}
