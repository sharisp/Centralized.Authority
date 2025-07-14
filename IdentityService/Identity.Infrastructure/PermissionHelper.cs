using Identity.Infrastructure.Repository;

namespace Identity.Infrastructure
{
    public class PermissionHelper
    {
        private BaseDbContext dbContext;

        public PermissionHelper(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string[]> GetPermissionsByUserId(long userId)
        {
            var permissions = await dbContext.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Permissions)
                .Distinct()
                .Select(t => (t.SystemName + "." + t.PermissionKey))
                .ToArrayAsync();
            return permissions;
        }

        public async Task<string[]> GetPermissionsBySystemNameAndUidAsync(long userId, string systemName)
        {
            var permissionQuery = dbContext.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Permissions);
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                permissionQuery = permissionQuery.Where(t => t.SystemName == systemName);
            }
            var permissions = await permissionQuery.Distinct()
                .Select(t => (t.SystemName + "." + t.PermissionKey))
                .ToArrayAsync();
            return permissions;
        }
        public async Task<Menu[]> GetMenusBySystemNameAndUid(long userId, string systemName)
        {
            var menus = await dbContext.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Menus).Where(t => t.SystemName == systemName)
                .Distinct()
                .ToArrayAsync();
            return menus;
        }
        public async Task<Menu[]> GetMenusWithBySystemNameAndUid(long userId, string systemName)
        {
            var menuQuery = dbContext.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Menus);
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                menuQuery = menuQuery.Where(t => t.SystemName == systemName);
            }

            var menus = await menuQuery.Distinct()
                   .ToArrayAsync();
            return menus;
        }
        public async Task<Menu[]> GetMenusWithPermissionBySystemNameAndUid(long userId, string systemName)
        {
            var menuQuery = dbContext.Users.Where(u => u.Id == userId)
                .SelectMany(u => u.Roles)
                .SelectMany(r => r.Menus);
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                menuQuery = menuQuery.Where(t => t.SystemName == systemName);
            }

            var menus = await menuQuery.Include(t => t.Permissions).Distinct()
                   .ToArrayAsync();
            return menus;
        }
        public async Task<bool> CheckPermissionAsync(string systemName, long userId, string permissionKey)
        {
            //   var permissionArr = await GetPermissionsByUserId(userId);

            var permissionArr = await RedisHelper.LRangeAsync($"{systemName}_user_permissions_{userId}", 0, -1);

            if (permissionArr == null || permissionArr.Length == 0)
            {

                //查询并写入，需要用分布式锁，防止重复写
                await LockHelper.RedisLock($"lock:user:permission:{userId}", async () =>
                {
                    // using var scope = _serviceScopeFactory.CreateScope();
                    // var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

                    var permissions = await GetPermissionsByUserId(userId); ;
                    //  foreach (var item in list)

                    await RedisHelper.RPushAsync($"{systemName}_user_permissions_{userId}", permissions);
                    await RedisHelper.ExpireAsync($"{systemName}_user_permissions_{userId}", 30 * 60);
                    permissionArr = permissions;
                }, async () =>
                {
                    //did not get lock, directly query the database
                    permissionArr = await GetPermissionsByUserId(userId);
                });
            }


            if (permissionArr.Any(x =>
                    x.Equals((systemName + "." + permissionKey), StringComparison.OrdinalIgnoreCase)) == false)
            {
                return false;
            }

            return true;
        }
    }
}