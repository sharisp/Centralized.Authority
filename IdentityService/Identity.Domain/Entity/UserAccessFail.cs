using Identity.Domain.Events;

namespace Identity.Domain.Entity
{
    public class UserAccessFail:BaseEntity
    {
        public long UserId { get; private set; }
        public User UserInfo { get; init; }
        public bool IsLocked { get; private set; }

        public DateTime? LockEndTime { get; private set; }
        public int FailCount { get; private set; }

        public bool CheckIfLocked()
        {
            if (IsLocked && LockEndTime != null && LockEndTime.Value < DateTime.Now)
            {
                Reset();
            }
            //finnaly check if still locked,after checking the passwrod
            if (IsLocked==false)
            {
                AddDomainEvent(new LoginSuccessEvent(UserInfo));
            }
            //
            return IsLocked;
        }
        public void Fail()
        {
            FailCount++;
            if (FailCount >= 3)
            {
                IsLocked = true;
                LockEndTime = DateTime.Now.AddMinutes(5);
            }
            AddDomainEvent(new LoginFailEvent(UserInfo));
        }

        public void Reset()
        {
            IsLocked = false;
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
            IsLocked = false;
            LockEndTime = null;
            FailCount = 0;
        }
    }
}
