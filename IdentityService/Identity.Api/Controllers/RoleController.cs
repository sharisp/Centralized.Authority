using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Entity;
using Identity.Infrastructure.Extensions;
using Identity.Infrastructure.Options;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleController(IValidator<CreateRoleDto> validator, RoleMapper mapper, RoleRepository repository, PermissionRepository permissionRepository, MenuRepository menuRepository, ICurrentUser currentUser) : ControllerBase
    {
        [HttpGet]
        [PermissionKey("Role.List")]
        public async Task<ActionResult<ApiResponse<List<Role>>>> List()
        {
            var query = repository.Query();
            var roles =await query.ToListAsync();
            return this.OkResponse(roles);
        }

        [HttpGet("Pagination")]
        [PermissionKey("Role.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Role>>>> ListByPagination(int pageIndex = 1, int pageSize = 10, string roleName = "", string description = "")
        {

            var query = repository.Query();
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                query = query.Where(t => t.RoleName.Contains(roleName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(t => !string.IsNullOrEmpty(t.Description) && t.Description.Contains(description.Trim()));

            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }

        [HttpGet("Detail/{id}")]
        [PermissionKey("Role.GetRoleDetail")]
        public async Task<ActionResult<ApiResponse<Role?>>> GetByRoleId(long id)
        {

            var query = repository.Query(true,true);
            var role = await query.Where(t => t.Id == id).FirstOrDefaultAsync();
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
            if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
            {
                var permissions = await permissionRepository.GetPermissionsByIds(dto.PermissionIds);
                info.AddPermissions(permissions);
            }
            if (dto.MenuIds != null && dto.MenuIds.Count > 0)
            {
                var meuns = await menuRepository.GetByIdsAsync(dto.MenuIds);

                info.AddMenus(meuns);
            }
            await repository.AddAsync(info);
            return this.OkResponse(info.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Role.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateRoleDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);


            var query = repository.Query(true, true);
            var role =  await query.FirstOrDefaultAsync(t => t.Id == id);
            if (role == null) return this.FailResponse("not found");

            if (!string.IsNullOrEmpty(dto.RoleName) && dto.RoleName != role.RoleName)
            {
                if ((await repository.GetByNameAsync(dto.RoleName)) != null)
                {
                    return this.FailResponse(" name already exists");
                }
            }
            mapper.UpdateDtoToRole(dto, role);
            role.Permissions.Clear();
            role.Menus.Clear();
            if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
            {
                var permissions = await permissionRepository.GetPermissionsByIds(dto.PermissionIds);
                role.AddPermissions(permissions);
            }
            if (dto.MenuIds != null && dto.MenuIds.Count > 0)
            {
                var meuns = await menuRepository.GetByIdsAsync(dto.MenuIds);

                role.AddMenus(meuns);
            }



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
           info.SoftDelete(currentUser);


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
