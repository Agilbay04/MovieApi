using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Role;

namespace MovieApi.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> FindByIdAsync(string id)
        {
            var role = await _context.Roles
                .Where(r => r.Id == id && r.Deleted == false)
                .FirstOrDefaultAsync() ?? throw new Exception("Role not found");
            return role;
        }

        public async Task<Role> FindByCodeAsync(string code)
        {
            var role = await _context.Roles
                .Where(r => r.Code == code && r.Deleted == false)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync() ?? throw new Exception("Role not found");
            return role;
        }

        public async Task<IEnumerable<Role>> FindAllAsync()
        {
            return await _context.Roles
                .Where(r => r.Deleted == false)
                .ToListAsync();
        }

        public async Task<Role> CreateAsync(CreateRoleRequest req)
        {
            if (req.Code == null)
                throw new Exception("Code is required");

            if (req.Name == null)
                throw new Exception("Name is required");

            var isCodeExists = await FindByCodeAsync(req.Code);
            if (isCodeExists != null)
                throw new Exception("Code already used");

            var role = new Role
            {
                Code = req.Code,
                Name = req.Name
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateAsync(UpdateRoleRequest req, string id)
        {
            if (id == null)
                throw new Exception("Id is required");

            if (req.Code == null)
                throw new Exception("Code is required");

            if (req.Name == null)
                throw new Exception("Name is required");

            var isRoleExists = await FindByIdAsync(id);

            if (isRoleExists != null)
            {
                if (isRoleExists.Code == req.Code)
                    throw new Exception("Code already used");
            }

            var role = await FindByIdAsync(id);
            role.Code = req.Code;
            role.Name = req.Name;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> DeleteAsync(string id)
        {
            var roleInUsed = await _context.Users
                .Where(u => u.RoleId == id && u.Deleted == false)
                .FirstOrDefaultAsync();
            
            if (roleInUsed != null)
                throw new Exception("Role in used");

            var role = await FindByIdAsync(id);
            role.Deleted = true;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}