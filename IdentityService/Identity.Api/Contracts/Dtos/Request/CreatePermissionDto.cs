using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Identity.Api.Contracts.Dtos.Request
{
    public class CreatePermissionDto
    {

        public string Title { get; set; }
        public string PermissionKey { get; set; }//user:add
        public string SystemName { get; set; } // for multi-system support, e.g., "Identity", "Order", etc.
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }

    }
}
