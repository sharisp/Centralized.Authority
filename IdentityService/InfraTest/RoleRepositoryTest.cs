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

        [Fact]
        public void TestWithMenu()
        {

            var role = new Role("TestRole");
            role.AddMenus(new List<Menu> {
            new Menu("Menu1", "Menu1", 0, 1, MenuType.Menu, "TES", null),
            new Menu("Menu2", "Menu2", 0, 1, MenuType.Menu, "TES", null)

            });
            context.Roles.Add(role);

            context.SaveChanges();

            var repo = new RoleRepository(context);
            var model = repo.Query().FirstOrDefault();
            Assert.NotNull(model);

            var modelNew = repo.Query(false,true).FirstOrDefault();
            Assert.NotNull(modelNew?.Menus);

        }

        [Fact]
        public void TestWithPermissions()
        {

            var role = new Role("TestRole");
            role.AddPermissions(new List<Permission> {
         new Permission("Perm1", "Perm1", "Test"),
         new Permission("Perm2", "Perm2", "Test"),

            });
        
            context.Roles.Add(role);

            context.SaveChanges();

            var repo = new RoleRepository(context);
            var model = repo.Query().FirstOrDefault();
            Assert.NotNull(model);

            var modelNew = repo.Query(true, false).FirstOrDefault();
            Assert.NotNull(modelNew?.Permissions);

        }

        [Fact]
        public void TestWithMenuPermissions()
        {

            var role = new Role("TestRole");
            role.AddPermissions(new List<Permission> {
         new Permission("Perm1", "Perm1", "Test"),
         new Permission("Perm2", "Perm2", "Test"),

            });
            role.AddMenus(new List<Menu> {
            new Menu("Menu1", "Menu1", 0, 1, MenuType.Menu, "TES", null),
            new Menu("Menu2", "Menu2", 0, 1, MenuType.Menu, "TES", null)

            });
            context.Roles.Add(role);

            context.SaveChanges();

            var repo = new RoleRepository(context);
            var model = repo.Query().FirstOrDefault();
            Assert.NotNull(model);

            var modelNew = repo.Query(true, true).FirstOrDefault();
            Assert.NotNull(modelNew?.Permissions);
            Assert.NotNull(modelNew?.Menus);
        }
    }
}
