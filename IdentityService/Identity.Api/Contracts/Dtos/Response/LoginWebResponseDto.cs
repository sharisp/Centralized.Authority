using Common.Jwt;
using Identity.Domain.Entity;

namespace Identity.Api.Contracts.Dtos.Response
{
    public class LoginWebResponseDto
    {
        public string UserName { get; set; }
        public string? NickName { get; set; }
        public long UserId { get; set; }
        public TokenWithExpireResponse Token { get; set; }
        public string[] Permissions { get; set; } = Array.Empty<string>();
        public Menu[] Menus { get; set; }
        public LoginWebResponseDto(string userName, string? nickName, long userId, TokenWithExpireResponse token, string[] permissions, Menu[] menus)
        {
            this.NickName = nickName;
            this.UserName = userName;
            this.UserId = userId;
            this.Token = token;
            this.Permissions = permissions;
            this.Menus = menus;
        }

    }
}
