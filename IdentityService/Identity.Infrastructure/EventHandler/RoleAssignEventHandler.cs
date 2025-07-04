﻿using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using MediatR;

namespace Identity.Infrastructure.EventHandler
{
    public class RoleAssignEventHandler : INotificationHandler<RoleAssignEvents>
    {
        private readonly BaseDbContext dbContext;

        public RoleAssignEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Handle(RoleAssignEvents notification, CancellationToken cancellationToken)
        {
            long cursor = 0;
            do
            {
                // 扫描匹配的 Key
                var scanResult =await RedisHelper.ScanAsync(cursor, "*_user_permissions_*", 1000);
                cursor = scanResult.Cursor;
                var keys = scanResult.Items;

                if (keys.Length > 0)
                {
                    // 批量删除
                    await RedisHelper.DelAsync(keys);
                }

            } while (cursor != 0);



        }
    }
}

