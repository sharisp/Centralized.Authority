using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Jwt
{
    public class TokenWithExpireResponse : TokenResponse
    {
        public DateTimeOffset AccessTokenExpiresAt { get; set; }

        public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    }
}
