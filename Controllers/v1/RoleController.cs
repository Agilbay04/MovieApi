using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Role;
using MovieApi.Responses;
using MovieApi.Responses.Role;
using MovieApi.Services.RoleService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly RoleMapper _roleMapper;

        public RoleController(IRoleService roleService, RoleMapper roleMapper)
        {
            _roleService = roleService;
            _roleMapper = roleMapper;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Find role by id")]
        [EndpointDescription("Find role by id from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<RoleResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<RoleResponse>>> FindRoleById(string id)
        {
            var role = await _roleService.FindByIdAsync(id);
            var roleDto = await _roleMapper.ToDto(role);
            var res = new BaseResponseApi<RoleResponse>(roleDto, "Find role by id successful");
            return Ok(res);
        }

        [HttpGet("code/{code}")]
        [EndpointSummary("Find role by code")]
        [EndpointDescription("Find role by code from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<RoleResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<RoleResponse>>> FindRoleByCode(string code)
        {
            var role = await _roleService.FindByCodeAsync(code);
            var roleDto = await _roleMapper.ToDto(role);
            var res = new BaseResponseApi<RoleResponse>(roleDto, "Find role by code successful");
            return Ok(res);
        }

        [HttpGet]
        [EndpointSummary("Find all roles")]
        [EndpointDescription("Find all roles from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<RoleResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<RoleResponse>>>> FindAllRolesAsync()
        {
            var roles = await _roleService.FindAllAsync();
            var rolesDto = await _roleMapper.ToDtos(roles.ToList());
            var res = new BaseResponseApi<IEnumerable<RoleResponse>>(rolesDto, "Find all roles successful");
            return Ok(res);
        }

        [HttpPost]
        [EndpointSummary("Create role")]
        [EndpointDescription("Create role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<RoleResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<RoleResponse>>> CreateRoleAsync(CreateRoleRequest request)
        {
            var role = await _roleService.CreateAsync(request);
            var roleDto = await _roleMapper.ToDto(role);
            var res = new BaseResponseApi<RoleResponse>(roleDto, "Create role successful");
            return Ok(res);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update role")]
        [EndpointDescription("Update role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<RoleResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<RoleResponse>>> UpdateRoleAsync(UpdateRoleRequest request, string id)
        {
            var role = await _roleService.UpdateAsync(request, id);
            var roleDto = await _roleMapper.ToDto(role);
            var res = new BaseResponseApi<RoleResponse>(roleDto, "Update role successful");
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete role")]
        [EndpointDescription("Delete role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<RoleResponse>>> DeleteRoleAsync(string id)
        {
            var role = await _roleService.DeleteAsync(id);
            var roleDto = await _roleMapper.ToDto(role);
            var res = new BaseResponseApi<RoleResponse>(roleDto, "Delete role successful");
            return Ok(res);
        }
    }
}