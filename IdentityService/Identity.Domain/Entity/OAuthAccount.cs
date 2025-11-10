using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class OAuthAccount : BaseAuditableEntity,IAggregateRoot
    {
        public long UserId { get; set; }

        public string Provider { get; set; } = "";
        public string ProviderUserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string ProviderUserName { get; set; } = "";
        public string Avatar { get; set; } = "";

      //  public User User { get; set; } = null!;

     public OAuthAccount(long userId, string provider, string providerUserId, string email, string providerUserName, string avatar="")
        {
            UserId = userId;
            Provider = provider;
            ProviderUserId = providerUserId;
            Email = email;
            ProviderUserName = providerUserName;
            Avatar = avatar;
        }
    }
}
