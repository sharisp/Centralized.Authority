using OAuthService.Options;

namespace Identity.Api.Contracts.Dtos.Request
{
    public record OAuthLoginDto
    {
        public string Code { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        // public string? Error { get; set; } = string.Empty;

        //uppercase enum causes deserialization issues

        // public OAuthProviderEnum Provider { get; set; } 
        public string Provider { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
    }
}
