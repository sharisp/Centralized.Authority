using Identity.Domain.Entity;

namespace Identity.Domain.Interfaces
{
    public interface IRoleRepository
    {
        IQueryable<Role> Query(bool isIncludePermisson = false, bool isIncludeMenu = false);
        Task AddAsync(Role role);
        Task<List<User>> GetUsersByRoleId(long roleId);
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetRolesByIds(List<long> ids);
        Task<Role?> GetByIdAsync(long id);

    }
}
