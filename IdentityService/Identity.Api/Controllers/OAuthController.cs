using Domain.SharedKernel.Interfaces;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Helper;
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
    public class OAuthController(OAuthDomainService oAuthDomainService, LoginHelper loginHelper, IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<LoginWebResponseDto>>> OAuthCallBack(string provider, string state, string code = "", string error = "", string systemName = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                return this.FailResponse(error);
            }
            Enum.TryParse<OAuthProviderEnum>(provider, true, out var oAuthProviderEnum);
            var user = await oAuthDomainService.OAuthLoginAsync(oAuthProviderEnum,state, code, error);
            if (user == null)
            {
                return this.FailResponse("OAuth login failed");
            }
            var info = await loginHelper.WebLogin(user, systemName);

            await unitOfWork.SaveChangesAsync();
            return this.OkResponse(info);
        }

        
    }
}
