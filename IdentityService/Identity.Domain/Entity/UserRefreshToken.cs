using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Entity
{
    public class UserRefreshToken: BaseAuditableEntity, IAggregateRoot
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpireAt { get; set; }


        public bool IsRevoked { get; set; } = false;

        public UserRefreshToken()
        {
            IsRevoked = false;
        }

    }
}
