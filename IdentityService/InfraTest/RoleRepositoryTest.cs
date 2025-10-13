using Identity.Domain.Entity;
using Identity.Domain.Enums;
using Identity.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraTest
{
    [Collection("Global Test Collection")]
    public class RoleRepositoryTest
    {
        BaseDbContext context;
        public RoleRepositoryTest(DBContextFixture dBContextFixture)
        {
            context = dBContextFixture.baseDbContext;
        }
        [Fact]
        public void Test1()
        {

            var role = new Role("TestRole");
            context.Roles.Add(role);
            context.SaveChanges();

            var repo = new RoleRepository(context);
            var model = repo.Query().FirstOrDefault();
            Assert.NotNull(model);

        }
    }
}
