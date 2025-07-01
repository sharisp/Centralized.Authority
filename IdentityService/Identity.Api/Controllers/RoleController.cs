using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Entity;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleController(IValidator<CreateRoleDto> validator, RoleMapper mapper, RoleRepository repository, PermissionRepository permissionRepository, BaseDbContext baseDbContext, IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [PermissionKey("Role.List")]
        public async Task<ActionResult<ApiResponse<List<Role>>>> List()
        {
            var roles = await baseDbContext.Roles.ToListAsync();
            return this.OkResponse(roles);
        }
        [AllowAnonymous]
        [HttpGet("Detail/{id}")]
        [PermissionKey("Role.GetRoleDetail")]
        public async Task<ActionResult<ApiResponse<Role?>>> GetByRoleId(long id)
        {
            var role = await baseDbContext.Roles.Include(t => t.Menus).Include(t=>t.Permissions).Where(t => t.Id == id).FirstOrDefaultAsync();
            return this.OkResponse(role);
        }
        [AllowAnonymous]
        [HttpGet("RoleMenuPermission/{id}")]
        [PermissionKey("Role.GetRoleMenuPermission")]
        public async Task<ActionResult<ApiResponse<Role?>>> GetRoleMenuPermission(long id)
        {
            var role = await baseDbContext.Roles.Include(t => t.Menus).ThenInclude(t => t.Permissions).
                Where(t => t.Id == id).FirstOrDefaultAsync();
            return this.OkResponse(role);
        }
        [HttpPost]
        [PermissionKey("Role.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreateRoleDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var role = await repository.GetByNameAsync(dto.RoleName);
            if (role != null) return this.FailResponse("RoleName exists");

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);
            return this.OkResponse(info.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Role.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateRoleDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByIdAsync(id);
            if (user == null) return this.FailResponse("not found");

            if (!string.IsNullOrEmpty(dto.RoleName) && dto.RoleName != user.RoleName)
            {
                if ((await repository.GetByNameAsync(dto.RoleName)) != null)
                {
                    return this.FailResponse(" name already exists");
                }
            }
            mapper.UpdateDtoToRole(dto, user);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Role.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return this.FailResponse(" not found");
            var users = await repository.GetUsersByRoleId(id);
            if (users.Count > 0)
            {
                return this.FailResponse("there are users in this role");
            }
            repository.DeleteRole(info);


            return this.OkResponse(id);
        }

        [HttpPost("Assign/{id}")]
        [PermissionKey("Permission.Assign")]
        public async Task<ActionResult<ApiResponse<string>>> Assign(long id, List<long> permissionIds)
        {
            if (permissionIds.Count == 0)
                return this.FailResponse(" permissions should not be empty");
            var role = await repository.GetByIdAsync(id);
            if (role == null) return this.FailResponse("role not found");

            var permissions = await permissionRepository.GetPermissionsByIds(permissionIds);
            role.AddPermissions(permissions);
            return this.OkResponse("successful");

        }

    }
}
