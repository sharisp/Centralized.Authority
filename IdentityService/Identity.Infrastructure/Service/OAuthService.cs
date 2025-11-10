using Microsoft.Extensions.DependencyInjection;
using OAuthService;
using OAuthService.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Identity.Infrastructure.Service
{
    public class OAuthService(IServiceProvider serviceProvider, IOAuthRepository oAuthRepository)
    {
        public async Task<OAuthAccount?> OAuthCallBack(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "")
        {
            var oAuthService = serviceProvider.GetRequiredKeyedService<IOAuthService>(oAuthProviderEnum);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            var info = await oAuthService.OAuthCallBack(state, code, error);
            if (info != null && info.Success && info.UserInfo != null)
            {
                //  var oAuthAccount = new OAuthAccount(-1, info.UserInfo.Provider, info.UserInfo.Id, info.UserInfo.Email, info.UserInfo.Name, info.UserInfo.Avatar);
                var oAuthAccount = await oAuthRepository.GetByProviderAndProviderUserIdAsync(info.UserInfo.Provider, info.UserInfo.Id);

                return oAuthAccount;

            }
            return null;
        }

        public IOAuthService GetOAuthProvideService(string provider)
        {

            OAuthProviderEnum oAuthProviderEnum = (OAuthProviderEnum)Enum.Parse(typeof(OAuthProviderEnum), provider);
            return serviceProvider.GetRequiredKeyedService<IOAuthService>(oAuthProviderEnum);

        }
    }
}