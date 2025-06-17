using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Contracts.Dtos.Request
{
    public class UserRolesDto : CreateUserRequestDto
    {
        public List<long> RoleIDList { get; set; }
    }
}
