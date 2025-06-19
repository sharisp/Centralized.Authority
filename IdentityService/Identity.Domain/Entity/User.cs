using Identity.Domain.Events;
using Identity.Domain.Interfaces;
using Identity.Domain.ValueObject;

namespace Identity.Domain.Entity
{
    public class User: BaseAuditableEntity, IAggregateRoot
    {
        //For EF Core Use
        private User()
        {
        }
        public string UserName { get;  set; } = default!;
        public string? RealName { get;  set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string? Email { get;  set; } = default!;
        public PhoneNumber? Phone { get;  set; } = default!;

        public string? NickName { get;  set; } = default!;

        public string? Description { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();

        public UserAccessFail AccessFail { get;private set; }
        public string? RefreshToken { get; private set; }
        public DateTimeOffset? RefreshTokenExpireAt { get; private set; }
        public User(string userName, string passwordHash, PhoneNumber? phone=null, string? email=null, string? nickName=null, string? realName = null, string? description = null)
        {

            PasswordHash = HashHelper.ComputeMd5Hash(passwordHash);
            NickName = nickName;
            UserName = userName;
            RealName = realName;
            Email = email;
            Phone = phone;
            Description = description;
            AccessFail = new UserAccessFail(this);
            AddDomainEvent(new UserAddEvents(this));
       
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == HashHelper.ComputeMd5Hash(password);
        }

        public void SetRefreshToken(string refreshToken, DateTimeOffset refreshTokenExpireAt)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpireAt = refreshTokenExpireAt; 
            AddDomainEvent(new UserUpdateEvents(this));
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(RefreshToken))
            {
                return false;
            }
            return RefreshToken == refreshToken && RefreshTokenExpireAt.HasValue && RefreshTokenExpireAt.Value > DateTimeOffset.UtcNow;
        }
        public void ClearRefreshToken()
        {
            RefreshToken = null;
            RefreshTokenExpireAt=DateTimeOffset.MinValue;
            AddDomainEvent(new UserUpdateEvents(this));
        }
        public void ChangePassword(string passwordHash)
        {
            PasswordHash =HashHelper.ComputeMd5Hash(passwordHash) ;
            AddDomainEvent(new UserUpdateEvents(this));
    }
        public void ChangeNickName(string nickName)
        {
            NickName = nickName;

            AddDomainEvent(new UserUpdateEvents(this));
        }

        public void ChangeRealName(string realName)
        {
            RealName = realName;

            AddDomainEvent(new UserUpdateEvents(this));
        }

        public void ChangeEmail(string email)
        {
            Email = email;

            AddDomainEvent(new UserUpdateEvents(this));
        }

        public void ChangePhone(PhoneNumber phone)
        {
            Phone = phone;

            AddDomainEvent(new UserUpdateEvents(this));
        }

    }


}