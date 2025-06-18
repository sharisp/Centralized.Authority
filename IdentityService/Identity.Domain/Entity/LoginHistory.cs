using Identity.Domain.Interfaces;
using Identity.Domain.ValueObject;

namespace Identity.Domain.Entity
{
    public class LoginHistory : BaseEntity, IAggregateRoot
    {
        private LoginHistory() { }

        public long UserId { get; init; }
        public string UserName { get; private set; }
        public string? LoginIp { get; private set; }
        public DateTime CreateTime { get; init; }

        public bool IsSuccess { get; private set; }
        public string? Message { get; private set; }
        public PhoneNumber? Phone { get; private set; }

        public LoginHistory(long userId, string userName, string? loginIp, bool isSuccess, string? message, PhoneNumber? phone)
        {
            UserId = userId;
            UserName = userName;
            LoginIp = loginIp;
            CreateTime = DateTime.Now;
            IsSuccess = isSuccess;
            Message = message;
            Phone = phone;
        }
    }
}
