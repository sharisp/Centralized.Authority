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
            var configs = await this.dbContext.SysConfigs.Where(t => t.IsActive == true).ToListAsync();
            return configs;
        }

        public Task<SystemConfig?> GetByConfigKey(string configKey, string? systemName)
        {
            throw new NotImplementedException();
        }
    }
}
