using Common.Jwt;
using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Enums;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDomainService userDomainService;
        private readonly IValidator<LoginRequestDto> validator;
        private readonly AuthenticationTokenResponse authenticationTokenResponse;
        public LoginController(UserDomainService userDomainService, AuthenticationTokenResponse authenticationTokenResponse, IValidator<LoginRequestDto> validator)
        {
            this.userDomainService = userDomainService;
            this.authenticationTokenResponse = authenticationTokenResponse;
            this.validator = validator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Login(LoginRequestDto loginRequestDto)
        {
            await ValidationHelper.ValidateModelAsync(loginRequestDto, validator);

            var (user, res) = await userDomainService.LoginByNameAndPwdAsync(loginRequestDto.UserName, loginRequestDto.Password);
            if (res != LoginResult.Success)
            {
                return BadRequest(ApiResponse<int>.Fail("login fail"));
            }
            else
            {
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);

                return Ok(ApiResponse<object>.Ok(new LoginResponseDto(
                    user.UserName,
                    user.NickName,
                  user.Id,
                    token
                )));
            }

        }

    }
}
