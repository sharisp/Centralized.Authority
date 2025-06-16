using Identity.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.EventHandler
{
    internal class UserAddEventHandler : INotificationHandler<UserAddEvents>
    {
        public Task Handle(UserAddEvents notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"create new user {notification.UserInfo.UserName}");
            return Task.CompletedTask;
        }
    }
}
