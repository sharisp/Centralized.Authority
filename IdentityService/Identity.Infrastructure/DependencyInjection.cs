using Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Identity.Domain.Services;
using Identity.Infrastructure.DbContext;
using Identity.Infrastructure.EventHandler;
using Identity.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 如果是多个DB，这儿可以改成 自定义的DBContext, 继承于DbContext即可
            services.AddDbContext<BaseDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });
         

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();


            services.AddScoped<UserDomainService>();

            // Register repositories

            // Register event handlers
            services.AddScoped<UserAddEventHandler>();

            services.AddScoped<UserUpdateEventHandler>();
            services.AddScoped<LoginFailEventHandler>();
            services.AddScoped<LoginSuccessEventHandler>();
            return services;
        }
    }
}
