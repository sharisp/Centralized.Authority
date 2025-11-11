using Identity.Domain.Entity;
using OAuthService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services
{
    public class OAuthDomainService(IOAuthHelper oAuthHelper, IOAuthRepository oAuthRepository, IUserRepository userRepository)
    {

        public async Task<User?> OAuthLoginAsync(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "")
        {
            var oauthResp = await oAuthHelper.OAuthCallBack(oAuthProviderEnum, state, code, error);
            if (oauthResp == null || oauthResp.Success == false || oauthResp.UserInfo == null)
            {
                return null; 
            }
            User? user = null;
            var oauthAccount = await oAuthRepository.GetByProviderAndProviderUserIdAsync(oAuthProviderEnum, oauthResp.UserInfo.Id);
            if (oauthAccount == null)
            {
                user = new User(oauthResp.UserInfo.Name, oauthResp.UserInfo.Email, "", null, null, null, null, true);
                await userRepository.AddUserAsync(user);
                var newOAuthAccount = new OAuthAccount(user.Id, oAuthProviderEnum.ToString(), oauthResp.UserInfo.Id, oauthResp.UserInfo.Email, oauthResp.UserInfo.Name, oauthResp.UserInfo.Avatar);
                await oAuthRepository.AddAsync(newOAuthAccount);
            }
            else
            {
                user = await userRepository.GetUserWithRolesByIdAsync(oauthAccount.UserId);

            }
            return user;
        }
    }
}