using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class UserRefreshToken
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpireAt { get; set; } 
        public DateTime CreateTime { get; set; } 

        public DateTime UpdateTime { get; set; } 
       
        public bool IsRevoked { get; set; } 

        public UserRefreshToken()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            IsRevoked = false;
        }

    }
}
