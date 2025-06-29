using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Interfaces;

namespace Identity.Domain
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
      
        public DateTimeOffset? CreateDateTime { get;  set; }

        public DateTimeOffset? UpdateDateTime { get;  set; }

        public DateTimeOffset? DeleteDateTime { get;  set; }

        public bool IsDel { get;  set; }

        public string? Description { get; set; }

        public long? CreatorUserId { get; set; }

        public long? UpdaterUserId { get;  set; }

        public long? DeleterUserId { get;  set; }

       
    }
}
