namespace Identity.Api.Contracts.Dtos.Request
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
        public long UserId { get; set; }
    }
}
