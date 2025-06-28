using Common.Jwt;
using FluentValidation;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(UserDomainService userDomainService,
        AuthenticationTokenResponse authenticationTokenResponse, IValidator<LoginRequestDto> validator, IValidator<RefreshTokenRequestDto> tokenValidator, IUserRepository userRepository, PermissionHelper permissionHelper) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginRequestDto)
        {
            await ValidationHelper.ValidateModelAsync(loginRequestDto, validator);

            var (user, res) =
                await userDomainService.LoginByNameAndPwdAsync(loginRequestDto.UserName, loginRequestDto.Password);
            if (res != LoginResult.Success)
            {
                return Ok(ApiResponse<LoginResponseDto>.Fail("login fail", 401));
            }
            else
            {
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);
                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return Ok(ApiResponse<LoginResponseDto>.Ok(new LoginResponseDto(
                    user.UserName,
                    user.NickName,
                    user.Id,
                    token
                )));
            }

        }
        /// <summary>
        /// for web login, return permissions and menus
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("WebLogin")]
        public async Task<ActionResult<ApiResponse<LoginWebResponseDto>>> LoginForWeb(LoginRequestWebDto loginRequestDto)
        {
            await ValidationHelper.ValidateModelAsync(loginRequestDto, validator);

            var (user, res) =
                await userDomainService.LoginByNameAndPwdAsync(loginRequestDto.UserName, loginRequestDto.Password);
            if (res != LoginResult.Success)
            {
                return Ok(ApiResponse<LoginWebResponseDto>.Fail("login fail", 401));
            }
            else
            {

                var menus = await permissionHelper.GetMenusBySystemNameAndUid(user.Id, loginRequestDto.SystemName);
                var permissions = await permissionHelper.GetPermissionsBySystemNameAndUidAsync(user.Id, loginRequestDto.SystemName);
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);
                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return Ok(ApiResponse<LoginWebResponseDto>.Ok(new LoginWebResponseDto(
                    user.UserName,
                    user.NickName,
                    user.Id,
                    token,
                    permissions,
                    menus

                )));
            }

        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> RefreshToken(RefreshTokenRequestDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, tokenValidator);

            var user = await userRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
            {
                return Ok(ApiResponse<int>.Fail("user not found", 400));
            }

            if (user.IsRefreshTokenValid(dto.RefreshToken))
            {
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);

                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return Ok(ApiResponse<object>.Ok(new LoginResponseDto(
                    user.UserName,
                    user.NickName,
                    user.Id,
                    token
                )));
            }

            return Ok(ApiResponse<LoginResponseDto>.Fail("RefreshToken not valid", 400));
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginOut()
        {
            var userId = Convert.ToInt64(this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Ok(ApiResponse<string>.Fail("user not found", 400));
            }

            user.ClearRefreshToken();

            return Ok(ApiResponse<string>.Ok("login out successfully"));
        }
    }

}
