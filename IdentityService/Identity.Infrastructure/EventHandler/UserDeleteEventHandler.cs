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
        public Task Handle(UserDeleteEvents notification, CancellationToken cancellationToken)
        {
            return RedisHelper.DelAsync($"user_permissions_{notification.UserInfo.Id}");
        }
    }
}