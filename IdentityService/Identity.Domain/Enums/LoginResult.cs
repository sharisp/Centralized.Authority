using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Enums
{
    public enum LoginResult
    {
        Success = 0,
        Fail = 1,
        UserNotFound = 2,
        PasswordError = 3,
        UserLocked = 4,
        UserDeleted = 5,
    }
}
