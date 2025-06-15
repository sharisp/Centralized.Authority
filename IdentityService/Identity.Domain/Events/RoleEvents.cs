using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using MediatR;

namespace Identity.Domain.Events
{
    public record RoleAddEvents(Role RoleInfo) : INotification;

    public record RoleChangeEvents(Role RoleInfo) : INotification;

    public record RoleDeleteEvents(Role RoleInfo) : INotification;

}


