using Identity.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interfaces
{
    public interface ISystemConfigRepository
    {
        Task<List<SystemConfig>> GetAllActive();
        Task<SystemConfig?> GetByConfigKey(string configKey,string? systemName);
    }
}
