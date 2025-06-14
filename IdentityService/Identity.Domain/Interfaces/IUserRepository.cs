using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;

namespace Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserById(long userId);
        public User GetUsers();
        public void AddUser();
        public void UpdateUser(User user);
        public void DeleteUser(long userId);
    }
}
