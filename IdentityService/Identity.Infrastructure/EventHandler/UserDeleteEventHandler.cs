using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using IdGen;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.EventHandler
{
    public class UserDeleteEventHandler : INotificationHandler<UserDeleteEvents>
    {
        public async Task Handle(UserDeleteEvents notification, CancellationToken cancellationToken)
        {
            //return RedisHelper.DelAsync($"user_permissions_{notification.UserInfo.Id}");
            long cursor = 0;
            do
            {
                // 扫描匹配的 Key
                var scanResult = await RedisHelper.ScanAsync(cursor, $"*_user_permissions__{notification.UserInfo.Id}", 1000);
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