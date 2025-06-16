using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuth.Entities.DTO
{
    public class UserRolesDto:CreateUserRequestDto
    {
        public List<long> RoleIDList { get; set; }
    }
}
