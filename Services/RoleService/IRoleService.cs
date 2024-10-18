using MovieApi.Entities;
using MovieApi.Requests.Role;

namespace MovieApi.Services.RoleService
{
    public interface IRoleService
    {
        Task<Role> FindByIdAsync(string id);
        Task<Role> FindByCodeAsync(string code);
        Task<IEnumerable<Role>> FindAllAsync();
        Task<Role> CreateAsync(CreateRoleRequest req);
        Task<Role> UpdateAsync(UpdateRoleRequest req, string id);
        Task<Role> DeleteAsync(string id);
    }
}