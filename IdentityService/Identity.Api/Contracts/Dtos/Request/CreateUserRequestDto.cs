﻿namespace Identity.Api.Contracts.Dtos.Request
{
    public class CreateUserRequestDto
    {
        //  public long Id { get; set; }
        public string UserName { get; set; }
        public string? RealName { get; set; }
        public string Email { get; set; }
      //  public string Password { get; set; }
        public List<long> RoleIds { get; set; } = new List<long>();
    }
}
