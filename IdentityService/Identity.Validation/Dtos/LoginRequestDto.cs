namespace Identity.Contracts.Dtos
{
    public record LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
