using Microsoft.Extensions.DependencyInjection;
using OAuthService.Options;

namespace Identity.Infrastructure.Service
{
    public class OAuthService(IServiceProvider serviceProvider)
    {
        public async Task<OAuthResponse?> OAuthCallBack(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "")
        {
            var oAuthService = serviceProvider.GetRequiredKeyedService<Domain.Interfaces.IOAuthService>(oAuthProviderEnum);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            var info = await oAuthService.OAuthCallBack(oAuthProviderEnum, state, code, error);
          
            return info;
        }

      
    }
}