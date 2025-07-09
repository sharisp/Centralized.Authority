using CSRedis;
using Domain.SharedKernel;
using Identity.Domain.Services;
using Identity.Infrastructure.DbContext;
using Identity.Infrastructure.EventHandler;
using Identity.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));


            services.AddDomainShardKernelCollection(configuration);
            // 如果是多个DB，这儿可以改成 自定义的DBContext, 继承于DbContext即可
            services.AddDbContext<BaseDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            #region 注入redis
            string redisConnectionString = configuration["RedisConnection"];
            CSRedisClient cSRedisClient = new CSRedisClient(redisConnectionString);
            RedisHelper.Initialization(cSRedisClient);
            #endregion

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<PermissionRepository>();
            services.AddScoped<MenuRepository>();
            services.AddScoped<SysRepository>();
            AppHelper.Init(configuration);
            services.AddScoped<UserDomainService>();
            services.AddScoped<PermissionHelper>();
            // Register repositories

            // Register event handlers
            services.AddScoped<UserAddEventHandler>();

            services.AddScoped<LoginFailEventHandler>();
            services.AddScoped<LoginSuccessEventHandler>();
            return services;
        }
    }
}
