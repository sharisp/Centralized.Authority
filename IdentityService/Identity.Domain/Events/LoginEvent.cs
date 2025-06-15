using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using MediatR;

namespace Identity.Domain.Events
{
    public record LoginSuccessEvent(User UserInfo) : INotification;

    public record LoginFailEvent(User UserInfo) : INotification;
}


