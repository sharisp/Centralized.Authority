using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Events;

namespace Identity.Domain.Entity
{
    public class UserAccessFail:BaseEntity
    {
        public long UserId { get; private set; }
        public User UserInfo { get; init; }
        private bool isLocked;

        public DateTime? LockEndTime { get; private set; }
        public int FailCount { get; private set; }

        public bool CheckIfLocked()
        {
            if (isLocked && LockEndTime != null && LockEndTime.Value < DateTime.Now)
            {
                Reset();
            }
            AddDomainEvent(new LoginSuccessEvent(UserInfo));
            return isLocked;
        }
        public void Fail()
        {
            FailCount++;
            if (FailCount >= 3)
            {
                isLocked = true;
                LockEndTime = DateTime.Now.AddMinutes(5);
            }
            AddDomainEvent(new LoginFailEvent(UserInfo));
        }

        public void Reset()
        {
            isLocked = false;
            LockEndTime = null;
            FailCount = 0;
        }

        private UserAccessFail()
        {
        }
        public UserAccessFail(User user)
        {
            UserInfo = user;
            UserId = user.Id;
            isLocked = false;
            LockEndTime = null;
            FailCount = 0;
        }
    }
}
