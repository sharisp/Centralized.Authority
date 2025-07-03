using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entity;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Identity.Infrastructure.DbContext
{
    internal class UserRepository : IUserRepository
    {
        private readonly BaseDbContext dbContext;
        public UserRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddUserAsync(User user)
        {
         await  dbContext.Users.AddAsync(user);
        
        }

        public  void DeleteUser(User user)
        {
            dbContext.Users.Remove(user);

        }

        public async Task<User?> GetUseByNameAsync(string userName)
        {
          var res=  await dbContext.Users.Include(t=>t.AccessFail).FirstOrDefaultAsync(t => t.UserName == userName);
          return res;
        }

        public async Task<User?> GetUserByIdAsync(long userId)
        {
            var res = await dbContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
            return res;
        }
        public async Task<User?> GetUserWithRolesByIdAsync(long userId)
        {
            var res = await dbContext.Users.Include(t=>t.Roles).FirstOrDefaultAsync(t => t.Id == userId);
            return res;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            var res = await dbContext.Users.ToListAsync();
            return res;
        }

        public void UpdateUserAsync(User user)
        {
            dbContext.Users.Update(user);
        }
    }
}
