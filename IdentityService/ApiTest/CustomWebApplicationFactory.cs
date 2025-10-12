using Domain.SharedKernel.Interfaces;
using Identity.Api;
using Identity.Infrastructure;
using Identity.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;
namespace ApiTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("XunitTesting");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<BaseDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });

        builder.Configure(app =>
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
            db.Database.EnsureCreated();
            db.Users.Add(new Identity.Domain.Entity.User("testuser", "11@11.com", "123456"));
            db.SaveChanges();
        });
    }
}



