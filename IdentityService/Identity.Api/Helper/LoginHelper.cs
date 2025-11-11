using Common.Jwt;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Entity;
using Identity.Infrastructure;

namespace Identity.Api.Helper
{
    public  class LoginHelper(PermissionHelper permissionHelper, AuthenticationTokenResponse authenticationTokenResponse)
    {
        public async Task<LoginWebResponseDto> WebLogin(User user,string systemName="")
        {
            var menus = await permissionHelper.GetMenusWithBySystemNameAndUid(user.Id, systemName);
            var permissions = await permissionHelper.GetPermissionsBySystemNameAndUidAsync(user.Id, systemName);
            var dict = new Dictionary<string, string>();
            dict.Add("permissions", string.Join(';', permissions));
            var roleNames = new List<string>();
            var roleIds = new List<long>();
            foreach (var role in user.Roles)
            {
                roleNames.Add(role.RoleName);

                roleIds.Add(role.Id);
            }
            dict.Add($"RoleIds", string.Join(',', roleIds));
            var token = authenticationTokenResponse.GetResponseToken(user.Id, user.UserName, roleNames, dict);
            user.SetRefreshToken(token.RefreshToken, token.RefreshTokenExpiresAt);
            return new LoginWebResponseDto(
                user.UserName,
                user.Email,
                user.NickName,
                user.Id,
                token,
                menus,
                permissions

            );
        }
    }
}
