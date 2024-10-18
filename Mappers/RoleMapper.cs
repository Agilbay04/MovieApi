using MovieApi.Entities;
using MovieApi.Responses.Role;

namespace MovieApi.Mappers
{
    public class RoleMapper
    {
        public async Task<RoleResponse> ToDto(Role role)
        {
            return await Task.Run(() =>
            {
                return new RoleResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    Code = role.Code,
                    CreatedAt = role.CreatedAt?.ToString("dd MMM yyyy HH:mm:ss"),
                    UpdatedAt = role.UpdatedAt?.ToString("dd MMM yyyy HH:mm:ss")
                };
            });
        }

        public async Task<IEnumerable<RoleResponse>> ToDtos(List<Role> roles) 
        {
            return await Task.Run(() =>
            {
                var listRoleResponse = new List<RoleResponse>();
                foreach (var role in roles)
                {
                    listRoleResponse.Add(ToDto(role).Result);
                }
                return listRoleResponse;
            });
        }
    }
}