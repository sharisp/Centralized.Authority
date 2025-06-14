using Identity.Domain;
using Identity.Domain.Events;
using Identity.Domain.Interfaces;
using Identity.Domain.ValueObject;

namespace Identity.Domain.Entity
{
    public class User: BaseEntity, IAggregateRoot
    {
        //For EF Core Use
        private User()
        {

        }
        public long UserId { get; private set; }
        public string UserName { get; private set; } = default!;
        public string? RealName { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public string? Email { get; private set; } = default!;
        public PhoneNumber Phone { get; private set; } = default!;

        public string NickName { get; private set; } = default!;



        public User(string userName, string passwordHash, PhoneNumber phone, string? email, string? nickName, string? realName)
        {
            UserId = IdGeneratorFactory.NewId();
            PasswordHash = passwordHash;
            NickName = nickName;
            UserName = userName;
            RealName = realName;
            Email = email;
            Phone = phone;

            AddDomainEvent(new UserAddEvents(this));
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
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