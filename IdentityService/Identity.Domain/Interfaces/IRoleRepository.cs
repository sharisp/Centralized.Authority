using Identity.Domain.Entity;

namespace Identity.Domain.Interfaces
{
    public interface IRoleRepository
    {
        void DeleteRole(Role role);
        Task<List<Role>> GetRolesByIds(List<long> ids);
        Task<Role?> GetByIdAsync(long id);
        Task<Role?> GetByNameAsync(string name);
        Task AddAsync(Role role);
        Task<List<User>> GetUsersByRoleId(long roleId);

    }
}
