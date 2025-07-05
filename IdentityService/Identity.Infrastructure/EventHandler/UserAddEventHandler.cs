using Identity.Domain.Events;
using MediatR;

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
