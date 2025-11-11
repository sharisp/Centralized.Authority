using OAuthService.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interfaces
{
    public interface IOAuthHelper
    {
        Task<OAuthResponse?> OAuthCallBack(OAuthProviderEnum oAuthProviderEnum, string state, string code = "", string error = "");

    }
}