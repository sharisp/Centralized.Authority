namespace Identity.Api.Contracts.Dtos.Request
{
    public class UserRolesDto : CreateUserRequestDto
    {
        public List<long> RoleIDList { get; set; }
    }
}
