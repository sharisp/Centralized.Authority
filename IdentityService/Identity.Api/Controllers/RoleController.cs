using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleController(IValidator<CreateRoleDto> validator, RoleMapper mapper, RoleRepository repository,PermissionRepository permissionRepository, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [PermissionKey("Role.Create")]
        public async Task<ActionResult<ApiResponse<CreateRoleDto>>> Create(CreateRoleDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByNameAsync(dto.RoleName);
            if (user != null) return BadRequest(ApiResponse<CreateRoleDto>.Fail("RoleName exists"));

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);

            return Ok(ApiResponse<CreateRoleDto>.Ok(dto));
        }

        [HttpPut("{id}")]
        [PermissionKey("Role.Update")]
        public async Task<ActionResult<ApiResponse<CreateRoleDto>>> Update(long id, [FromBody] CreateRoleDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByIdAsync(id);
            if (user == null) return NotFound(ApiResponse<CreateRoleDto>.Fail("not found"));

            if (!string.IsNullOrEmpty(dto.RoleName) && dto.RoleName != user.RoleName)
            {
                if ((await repository.GetByNameAsync(dto.RoleName)) != null)
                {
                    return BadRequest(ApiResponse<CreateRoleDto>.Fail(" name already exists"));
                }
            }
            mapper.UpdateDtoToRole(dto, user);

            return Ok(ApiResponse<CreateRoleDto>.Ok(dto));
        }

        [HttpDelete("{id}")]
        [PermissionKey("Role.Delete")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return NotFound(ApiResponse<bool>.Fail(" not found"));
            var users= await repository.GetUsersByRoleId(id);
            if (users.Count>0)
            {
                return BadRequest(ApiResponse<string>.Fail("there are users in this role"));
            }
            repository.DeleteRole(info);


            return Ok(ApiResponse<string>.Ok("create error"));
        }

        [HttpPost("Assign/{id}")]
        [PermissionKey("Permission.Assign")]
        public async Task<ActionResult<ApiResponse<bool>>> Assign(long id, List<long> permissionIds)
        {
            if (permissionIds.Count == 0)
                return BadRequest(ApiResponse<bool>.Fail(" permissions should not be empty"));
            var role = await repository.GetByIdAsync(id);
            if (role == null) return NotFound(ApiResponse<bool>.Fail("role not found"));

            var permissions =await permissionRepository.GetPermissionsByIds(permissionIds);
            role.AddPermissions(permissions);
            return Ok(ApiResponse<string>.Ok("success"));
          
        }

    }
}
