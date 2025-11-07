using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class OAuthAccounts : BaseAuditableEntity
    {
      //  public long UserId { get; set; }

        public string Provider { get; set; } = "";
        public string ProviderUserId { get; set; } = "";
        public string Email { get; set; } = "";
        public string ProviderUserName { get; set; } = "";
        public string Avatar { get; set; } = "";

        public User User { get; set; } = null!;
    }
}
