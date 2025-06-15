using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Identity.Infrastructure
{
    public class CurrentUser(IHttpContextAccessor contextAccessor) : ICurrentUser
    {
        public long? UserId =>
            long.TryParse(contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
                ? id : null;


    }
}
