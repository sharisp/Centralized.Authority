using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OAuthService.Options;

namespace Identity.Infrastructure.Service
{
    public class OAuthHelper(IServiceProvider serviceProvider, ILogger<OAuthHelper> logger) : IOAuthHelper
    {
        public async Task<OAuthResponse?> OAuthCallBack(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "")
        {
            logger.LogDebug($"OAuthCallBack called with provider: {oAuthProviderEnum}, state: {state}, code: {code}, error: {error}");
            var oAuthService = serviceProvider.GetRequiredKeyedService<OAuthService.IOAuthService>(oAuthProviderEnum);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            var info = await oAuthService.OAuthCallBack(state, code, error);
            logger.LogDebug($"oAuthService callback info return : {info}");
            return info;
        }

      
    }
}