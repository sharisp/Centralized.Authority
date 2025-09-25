using Identity.Infrastructure.Repository;

namespace Identity.Infrastructure.DbContext
{
    internal class UserRepository : IUserRepository
    {
        private readonly BaseDbContext dbContext;
        public UserRepository(BaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<User> Query(bool isIncludeRole = false)
        {
            var query = dbContext.Users.AsQueryable();
            if (isIncludeRole)
            {
                query = query.Include(t => t.Roles);
            }
           
            return query;
        }
        public async Task AddUserAsync(User user)
        {
         await  dbContext.Users.AddAsync(user);
        
        }

        

        public async Task<User?> GetUserByNameAsync(string userName)
        {
          var res=  await dbContext.Users.Include(t=>t.AccessFail).FirstOrDefaultAsync(t => t.UserName == userName);
          return res;
        }
        public async Task<User?> GetUserWithRolesByNameAsync(string userName)
        {
            var res = await dbContext.Users.Include(t => t.AccessFail).Include(t=>t.Roles).FirstOrDefaultAsync(t => t.UserName == userName);
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
