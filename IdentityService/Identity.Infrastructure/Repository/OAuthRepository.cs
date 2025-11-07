using Identity.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using OAuthService;
using OAuthService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repository
{
    // public class OAuthRepository([FromKeyedServices(OAuthProviderEnum.Google)] IOAuthService googleOAuthService)
    public class OAuthRepository( BaseDbContext dbContext)
    {
       


        public async Task AddAsync(OAuthAccount oAuthAccount)
        {
            await dbContext.OAuthAccounts.AddAsync(oAuthAccount);

        }


        public async Task<OAuthAccount?> GetByProviderAndProviderUserIdAsync(string provider,string providerUserId)
        {
            var res = await dbContext.OAuthAccounts.FirstOrDefaultAsync(t=>t.ProviderUserId== providerUserId&&t.Provider== provider);
            return res;
        }
       
    }
}
