using Domain.SharedKernel.HelperFunctions;
using Identity.Domain.Entity;
using Identity.Domain.Enums;
using Identity.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace InfraTest
{
    [Collection("Global Test Collection")]
    public class MenuRespTest
    {
        BaseDbContext context;
        public MenuRespTest(DBContextFixture dBContextFixture)
        {
            context = dBContextFixture.baseDbContext;
        }
        [Fact]
        public void Test1()
        {


            context.Menus.Add(new Menu("Menu1", "Menu1", 0, 1, MenuType.Menu, "TES", null));
            context.SaveChanges();

            var repo = new MenuRepository(context);
            var menu = repo.Query().FirstOrDefault();
            Assert.NotNull(menu);

        }
    }
}