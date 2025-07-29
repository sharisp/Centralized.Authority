namespace Identity.Api.Contracts.Dtos.Request
{
    public record ChangePwdDto
    {
       
        public string? UserName { get; set; }

        public string OldPassword { get; set; }
      
        public string NewPassword { get; set; }
       
        public string ConfirmNewPassword { get; set; }
    }
}
