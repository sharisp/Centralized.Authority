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
        public Task<User?> GetUserByIdAsync(long userId);
        public Task<User?> GetUseByNameAsync(string userName);
        public Task<List<User>> GetUsersAsync();
        public  Task AddUserAsync(User user);
        public void UpdateUserAsync(User user);
        public void DeleteUser(User user);
    }
}
