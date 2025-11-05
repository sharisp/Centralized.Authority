using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        private readonly BaseDbContext dbContext;

        public SystemConfigRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<SystemConfig>> GetAllActive()
        {
            var configs = await this.dbContext.SysConfigs.AsNoTracking().Where(t => t.IsActive == true).ToListAsync();
            return configs;
        }

        public async Task<SystemConfig?> GetByConfigKey(string configKey, string? systemName)
        {
            var query = this.dbContext.SysConfigs.AsQueryable().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                query = query.Where(t => t.SystemName == systemName);
            }
            query = query.Where(t => t.ConfigKey == configKey);
            var config = await query.FirstOrDefaultAsync();
            return config;
        }
    }
}
