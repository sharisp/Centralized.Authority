using Common.Jwt;

namespace Identity.Api.Contracts.Dtos.Response
{
    public class LoginResponseDto
    {
        public string UserName { get; set; }
        public string? NickName { get; set; }
        public long UserId { get; set; }
        public TokenWithExpireResponse Token { get; set; }

        public LoginResponseDto(string userName, string? nickName, long userId, TokenWithExpireResponse token)
        {
            this.NickName = nickName;
            this.UserName = userName;
            this.UserId = userId;
            this.Token = token;
        }

    }
}
