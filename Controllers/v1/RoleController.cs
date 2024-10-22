using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Role;
using MovieApi.Responses.Role;
using MovieApi.Services.RoleService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
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
        [ProducesResponseType(type: typeof(RoleResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> FindRoleById(string id)
        {
            try
            {
                var role = await _roleService.FindByIdAsync(id);
                var roleDto = await _roleMapper.ToDto(role);
                return Ok(roleDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("code/{code}")]
        [EndpointSummary("Find role by code")]
        [EndpointDescription("Find role by code from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(RoleResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> FindRoleByCode(string code)
        {
            try
            {
                var role = await _roleService.FindByCodeAsync(code);
                var roleDto = await _roleMapper.ToDto(role);
                return Ok(roleDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [EndpointSummary("Find all roles")]
        [EndpointDescription("Find all roles from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(List<RoleResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> FindAllRolesAsync()
        {
            try
            {
                var roles = await _roleService.FindAllAsync();
                var rolesDto = await _roleMapper.ToDtos(roles.ToList());
                return Ok(rolesDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [EndpointSummary("Create role")]
        [EndpointDescription("Create role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(RoleResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<RoleResponse>> CreateRoleAsync(CreateRoleRequest request)
        {
            try
            {
                var role = await _roleService.CreateAsync(request);
                var roleDto = await _roleMapper.ToDto(role);
                return Ok(roleDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update role")]
        [EndpointDescription("Update role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(RoleResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<RoleResponse>> UpdateRoleAsync(UpdateRoleRequest request, string id)
        {
            try
            {
                var role = await _roleService.UpdateAsync(request, id);
                var roleDto = await _roleMapper.ToDto(role);
                return Ok(roleDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete role")]
        [EndpointDescription("Delete role from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<RoleResponse>> DeleteRoleAsync(string id)
        {
            try
            {
                var role = await _roleService.DeleteAsync(id);
                var roleDto = await _roleMapper.ToDto(role);
                return Ok(roleDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}