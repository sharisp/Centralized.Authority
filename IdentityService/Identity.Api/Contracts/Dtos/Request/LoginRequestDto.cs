namespace Identity.Api.Contracts.Dtos.Request
{
    public record LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public record LoginRequestWebDto: LoginRequestDto
    {
        public string SystemName { get; set; }
    }
}
