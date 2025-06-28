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
    public class MenuRepository 
    {
        private readonly BaseDbContext dbContext;

        public MenuRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Delete(Menu info)
        {
            dbContext.Menus.Remove(info);
        }

        public async Task AddAsync(Menu info)
        {
            await dbContext.Menus.AddAsync(info);

        }

        public async Task<List<Role>> GetRolesByMenuId(long menuId)
        {
            var menu = await dbContext.Menus
                .Include(t => t.Roles)
                .FirstAsync(t => t.Id == menuId);

            return menu.Roles.ToList();
        }

    
        public async Task<List<Menu>> GetByIdsAsync(List<long> ids)
        {
            var res = await dbContext.Menus.Where(t => ids.Contains(t.Id)).ToListAsync();
            return res;
        }
        public async Task<Menu?> GetByIdAsync(long id)
        {
            var res = await dbContext.Menus.FirstOrDefaultAsync(t => t.Id == id);
            return res;
        }
        public async Task<Menu?> GetByPathAsync(string path, string systemName)
        {
            var res = await dbContext.Menus.FirstOrDefaultAsync(t => t.Path.ToLower().Trim() == path.ToLower().Trim() && t.SystemName.ToLower().Trim() == systemName.ToLower().Trim());
            return res;
        }

    }
}
