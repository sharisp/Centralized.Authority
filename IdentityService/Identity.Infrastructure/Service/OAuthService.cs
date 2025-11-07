using Microsoft.Extensions.DependencyInjection;
using OAuthService;
using OAuthService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Service
{
    public class OAuthService(IServiceProvider serviceProvider)
    {
        public async Task<OAuthResponse?> OAuthCallBack(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "")
        {
            var oAuthService = serviceProvider.GetRequiredKeyedService<IOAuthService>(oAuthProviderEnum);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            var info = await oAuthService.OAuthCallBack(state, code, error);
            return info;
        }
    }
}
