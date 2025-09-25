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
                return this.FailResponse("login fail", 401);
            }
            else
            {
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);
                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return this.OkResponse(new LoginResponseDto(
                    user.UserName,
                    user.Email,
                    user.NickName,
                    user.Id,
                    token
                ));
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
                await userDomainService.LoginByNameAndPwdAsync(loginRequestDto.UserName.Trim(), loginRequestDto.Password.Trim());
            if (res != LoginResult.Success)
            {
                return this.FailResponse("login fail", 401);
            }
            else
            {

                var menus = await permissionHelper.GetMenusWithBySystemNameAndUid(user.Id, loginRequestDto.SystemName);
                var permissions = await permissionHelper.GetPermissionsBySystemNameAndUidAsync(user.Id, loginRequestDto.SystemName);
                var dict = new Dictionary<string, string>();
                dict.Add("permissions", string.Join(';', permissions));
                var roleNames = new List<string>();

                foreach (var role in user.Roles)
                {
                    roleNames.Add(role.RoleName);
                    dict.Add($"RoleId", role.Id.ToString());
                }
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName, roleNames, dict);
                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return this.OkResponse(new LoginWebResponseDto(
                    user.UserName,
                    user.Email,
                    user.NickName,
                    user.Id,
                    token,
                    menus,
                    permissions

                ));
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
                return this.FailResponse("user not found");
            }

            if (user.IsRefreshTokenValid(dto.RefreshToken))
            {
                var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName);

                user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
                return this.OkResponse(new LoginResponseDto(
                    user.UserName,
                    user.Email,
                    user.NickName,
                    user.Id,
                    token
                ));
            }

            return this.FailResponse("RefreshToken not valid");
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<ApiResponse<string>>> LoginOut()
        {
            var userId = Convert.ToInt64(this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return this.FailResponse("user not found");
            }

            user.ClearRefreshToken();

            return this.OkResponse("login out successfully");
        }
    }

}
