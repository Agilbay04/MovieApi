using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Role;
using MovieApi.Services.UserService;

namespace MovieApi.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        private readonly IUserService _userService;

        public RoleService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Role> FindByIdAsync(string id)
        {
            var role = await _context.Roles
                .Where(r => r.Id == id && r.Deleted == false)
                .FirstOrDefaultAsync() ?? throw new DllNotFoundException("Role not found");
            return role;
        }

        public async Task<Role> FindByCodeAsync(string code)
        {
            var role = await _context.Roles
                .Where(r => r.Code == code && r.Deleted == false)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync() ?? throw new DllNotFoundException("Role not found");
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
                throw new BadHttpRequestException("Code is required");

            if (req.Name == null)
                throw new BadHttpRequestException("Name is required");

            var isCodeExists = await FindByCodeAsync(req.Code);
            if (isCodeExists != null)
                throw new BadHttpRequestException("Code already used");

            var role = new Role
            {
                Code = req.Code,
                Name = req.Name,
                CreatedBy = _userService.GetUserId(),
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateAsync(UpdateRoleRequest req, string id)
        {
            if (id == null)
                throw new BadHttpRequestException("Id is required");

            if (req.Code == null)
                throw new BadHttpRequestException("Code is required");

            if (req.Name == null)
                throw new BadHttpRequestException("Name is required");

            var isRoleExists = await FindByIdAsync(id);

            if (isRoleExists != null)
            {
                if (isRoleExists.Code == req.Code)
                    throw new BadHttpRequestException("Code already used");
            }

            var role = await FindByIdAsync(id);
            role.Code = req.Code;
            role.Name = req.Name;
            role.UpdatedBy = _userService.GetUserId();
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
                throw new BadHttpRequestException("Role in used");

            var role = await FindByIdAsync(id);
            role.Deleted = true;
            role.UpdatedBy = _userService.GetUserId();
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}