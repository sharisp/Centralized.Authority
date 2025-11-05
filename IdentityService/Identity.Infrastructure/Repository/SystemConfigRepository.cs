using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class SystemConfigRepository : ISystemConfigRepository
    {
        public Task<List<SystemConfig>> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Task<SystemConfig?> GetByConfigKey(string configKey, string? systemName)
        {
            throw new NotImplementedException();
        }
    }
}
