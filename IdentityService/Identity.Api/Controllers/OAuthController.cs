using Domain.SharedKernel.Interfaces;
using Identity.Api.Contracts.Dtos.Request;
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
    public class OAuthController(OAuthDomainService oAuthDomainService, LoginHelper loginHelper, IUnitOfWork unitOfWork, ILogger<OAuthController> logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginWebResponseDto>>> OAuthCallBack(OAuthLoginDto oAuthLoginDto)
        {
            logger.LogDebug($"OAuth login request received : {oAuthLoginDto}");
            if (!Enum.TryParse<OAuthProviderEnum>(oAuthLoginDto.Provider, ignoreCase: true, out var providerEnum))
            {
                return this.FailResponse("OAuth login failed,wrong provider");
            }
            var (user, errMsg) = await oAuthDomainService.OAuthLoginAsync(providerEnum, oAuthLoginDto.State, oAuthLoginDto.Code);
            if (user == null)
            {
                return this.FailResponse("OAuth login failed," + errMsg);
            }

            await unitOfWork.SaveChangesAsync();
            var info = await loginHelper.WebLogin(user, oAuthLoginDto.SystemName);

            return this.OkResponse(info);
        }


    }
}
