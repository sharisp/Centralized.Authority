using Identity.Domain.Enums;

namespace Identity.Api.Contracts.Dtos.Request
{
    public class CreateMenuDto
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public long ParentId { get; set; }
        public string? Component { get; set; }
        public string? Icon { get; set; }
        public int Sort { get; set; }
        public MenuType Type { get; set; }
        public Uri? ExternalLink { get; set; }
        public string SystemName { get;  set; }
    }
}
