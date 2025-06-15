using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.EventHandler
{
    public class LoginSuccessEventHandler : INotificationHandler<LoginSuccessEvent>
    {
        private readonly BaseDbContext dbContext;

        public LoginSuccessEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public Task Handle(LoginSuccessEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.UserInfo.UserName + $"-login success uid={notification.UserInfo.Id}");
            return Task.CompletedTask;
        }
    }

    public class LoginFailEventHandler : INotificationHandler<LoginFailEvent>
    {
        private readonly BaseDbContext dbContext;

        public LoginFailEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public Task Handle(LoginFailEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.UserInfo.UserName + $"-login fail uid={notification.UserInfo.Id}");
            return Task.CompletedTask;
        }
    }
}