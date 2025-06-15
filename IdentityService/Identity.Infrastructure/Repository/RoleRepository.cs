using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;

namespace Identity.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BaseDbContext dbContext;

        public RoleRepository(BaseDbContext dbContext)
        {
            this.dbContext= dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void DeleteRole(Role role)
        {
           dbContext.Roles.Remove(role);
        }

    }
}
