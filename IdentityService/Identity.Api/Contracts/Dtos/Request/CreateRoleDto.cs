
namespace Identity.Api.Contracts.Dtos.Request
{
    public class CreateRoleDto
    {
      //  public long Id { get; set; }
        public string RoleName { get; set; }
        public string? Description { get; set; }

        public List<long> PermissionIds { get; set; } = new List<long>();
        public List<long> MenuIds { get; set; } = new List<long>();
    }
}
