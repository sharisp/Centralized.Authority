using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Common.Jwt
{
    public class AuthenticationExtensions(IOptionsSnapshot<JwtOptions> optionsSnapshot)
    {
        public TokenWithExpireResponse GetResponseToken(long userID,string userName)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userName));
            //I do not want to use roles as claims
             var jwtExpire = DateTime.Now.AddSeconds(optionsSnapshot.Value.ExpiresMinutes);

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsSnapshot.Value.SecKey));

            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescription =
                new JwtSecurityToken(claims: claims, expires: jwtExpire, signingCredentials: credentials);
            var jwToken = new JwtSecurityTokenHandler().WriteToken(tokenDescription);
            return new TokenWithExpireResponse
            {
                AccessToken = jwToken,
                RefreshToken = Guid.NewGuid().ToString(),
                AccessTokenExpiresAt = jwtExpire,
                RefreshTokenExpiresAt = DateTimeOffset.Now.AddHours(optionsSnapshot.Value.RefreshTokenExpiresHours)
            };
        }
    }
}
