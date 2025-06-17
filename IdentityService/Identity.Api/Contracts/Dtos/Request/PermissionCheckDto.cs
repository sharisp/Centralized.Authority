namespace Identity.Api.Contracts.Dtos.Request
{
    public class PermissionCheckDto
    {
        public string PermissionKey { get; set; } // e.g., "user:add"
        public long UserId { get; set; } // The ID of the user to check permissions for
        public string SystemName { get; set; }  // Optional system name for multi-system support
    }
}
