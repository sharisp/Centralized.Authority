using Identity.Api.Contracts.Dtos.Response;
using Identity.Infrastructure;
using Identity.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuthService;
using OAuthService.Options;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OAuthController([FromKeyedServices(OAuthProviderEnum.Google)]IOAuthService googleOAuthService) : ControllerBase
    {
        [HttpGet("google")]
        public async Task<ActionResult<ApiResponse<OAuthUserInfo>>> GoogleOAuthCallBack(string state, string code = "", string error = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                return this.FailResponse(error);
            }
            var info = await googleOAuthService.OAuthCallBack(state, code, error);
            return this.OkResponse(info.UserInfo);
        }
    }
}
