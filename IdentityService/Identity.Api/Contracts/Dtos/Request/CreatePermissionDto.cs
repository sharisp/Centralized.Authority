using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuth.Entities.DTO
{
    public class CreatePermissionDto
    {

        public string Title { get; set; }
        public string PermissionKey { get; set; }//user:add
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }

    }
}
