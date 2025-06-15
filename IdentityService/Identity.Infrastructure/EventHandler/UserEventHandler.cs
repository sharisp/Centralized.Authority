using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using MediatR;

namespace Identity.Infrastructure.EventHandler
{
    public class UserAddEventHandler : INotificationHandler<UserAddEvents>
    {
        private readonly BaseDbContext dbContext;
        public UserAddEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task Handle(UserAddEvents notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.UserInfo.UserName + $"-新增uid={notification.UserInfo.Id}");
            return Task.CompletedTask;
        }
    }

    public class UserUpdateEventHandler : INotificationHandler<UserUpdateEvents>
    {
        private readonly BaseDbContext dbContext;
        public UserUpdateEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Task Handle(UserUpdateEvents notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.UserInfo.UserName + $"-修改uid={notification.UserInfo.Id}");
            return Task.CompletedTask;
        }
    }
}
