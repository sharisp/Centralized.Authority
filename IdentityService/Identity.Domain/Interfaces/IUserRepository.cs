using Identity.Domain.Entity;

namespace Identity.Domain.Interfaces
{
    public interface IUserRepository
    {
         Task<User?> GetUserByIdAsync(long userId);
         Task<User?> GetUseByNameAsync(string userName);
         Task<List<User>> GetUsersAsync();
          Task AddUserAsync(User user);
         void UpdateUserAsync(User user);
     
        Task<User?> GetUserWithRolesByIdAsync(long userId);


    }
}
