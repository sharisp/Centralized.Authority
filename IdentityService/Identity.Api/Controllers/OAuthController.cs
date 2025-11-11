using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Services;
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
    public class OAuthController(OAuthDomainService oAuthDomainService ) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<OAuthUserInfo>>> OAuthCallBack(string provider,string state, string code = "", string error = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                return this.FailResponse(error);
            }
            Enum.TryParse<OAuthProviderEnum>(provider, true, out var oAuthProviderEnum);
            var info = await oAuthDomainService.OAuthLoginAsync(oAuthProviderEnum, code, error);
            
            return this.OkResponse(info);
        }
    }
}
