using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;
using Identity.Domain.ValueObject;

namespace Identity.Infrastructure.EventHandler
{
    public class LoginSuccessEventHandler : INotificationHandler<LoginSuccessEvent>
    {
        private readonly BaseDbContext dbContext;

        public LoginSuccessEventHandler(BaseDbContext dbContext, ICurrentUser currentUser)
        {
            this.dbContext = dbContext;
        }


        public async Task Handle(LoginSuccessEvent notification, CancellationToken cancellationToken)
        {
            dbContext.LoginHistories.Add(new LoginHistory(notification.UserInfo.Id, notification.UserInfo.UserName, null, true, "login success", new PhoneNumber("61", "123456")));
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class LoginFailEventHandler : INotificationHandler<LoginFailEvent>
    {
        private readonly BaseDbContext dbContext;

        public LoginFailEventHandler(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task Handle(LoginFailEvent notification, CancellationToken cancellationToken)
        {
            dbContext.LoginHistories.Add(new LoginHistory(notification.UserInfo.Id, notification.UserInfo.UserName,null,false,"login fail",new PhoneNumber("61","123456")));
           await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}