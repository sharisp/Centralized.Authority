using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class SystemConfig: BaseAuditableEntity, IAggregateRoot
    {
        public string? SystemName { get; set; }
        public string ConfigKey { get; set; } = string.Empty;
        public string ConfigValue { get; set; } = string.Empty;
        public string ValueType { get; set; } = "string";
      
        public bool IsActive { get; set; } = true;

    }
}
