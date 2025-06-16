namespace Identity.Api.Contracts.Dtos.Response
{
    public class UserResponseDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}
