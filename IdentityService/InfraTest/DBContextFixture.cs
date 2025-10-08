using Identity.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraTest
{
    public class DBContextFixture
    {
      public  BaseDbContext baseDbContext;
        public DBContextFixture()
        {
            var options = new DbContextOptionsBuilder<BaseDbContext>().UseInMemoryDatabase("TestDb")
    .Options;
            baseDbContext = new BaseDbContext(options);
        }
    }
}
