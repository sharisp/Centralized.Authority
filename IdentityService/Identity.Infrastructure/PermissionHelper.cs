using Identity.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure
{
    public class PermissionHelper
    {
        private BaseDbContext dbContext;

        public PermissionHelper(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CheckPermissionAsync(string systemName, long userId, string permissionKey)
        {
            var permissionArr = await RedisHelper.LRangeAsync($"{systemName}_user_permissions_{userId}", 0, -1);

            if (permissionArr == null || permissionArr.Length == 0)
                //查询并写入，需要用分布式锁，防止重复写
                await LockHelper.RedisLock($"lock:user:permission:{userId}", async () =>
                {
                    // using var scope = _serviceScopeFactory.CreateScope();
                    // var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

                    var userIDInt = Convert.ToInt64(userId);



                    var permissions = await dbContext.Users.Where(u => u.Id == userIDInt)
                        .SelectMany(u => u.Roles)
                        .SelectMany(r => r.Permissions)
                        .Distinct()
                        .Select(t => (t.SystemName + "." + t.PermissionKey))
                        .ToListAsync();

                    ;
                    //  foreach (var item in list)
                    foreach (var permission in permissions)
                    {
                        await RedisHelper.RPushAsync($"{systemName}_user_permissions_{userId}", permission);
                    }

                    permissionArr = permissions.ToArray();
                });

            if (permissionArr.Any(x =>
                    x.Equals((systemName + "." + permissionKey), StringComparison.OrdinalIgnoreCase)) == false)
            {
                return false;
            }

            return true;
        }
    }
}