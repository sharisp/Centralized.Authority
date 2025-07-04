using Identity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    public class SysRepository
    {
        private readonly BaseDbContext _context;

        public SysRepository(BaseDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Sys>> GetAllSys()
        {
            return await _context.Sys.ToListAsync();
        }


    }
}
