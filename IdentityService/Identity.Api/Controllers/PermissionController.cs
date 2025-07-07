using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Entity;
using Identity.Infrastructure;
using Identity.Infrastructure.Extensions;
using Identity.Infrastructure.Options;
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
    public class PermissionController(IValidator<CreatePermissionDto> validator, PermissionMapper mapper, PermissionRepository repository,ICurrentUser currentUser) : ControllerBase
    {
        [HttpGet]
        [PermissionKey("Permission.List")]
        public async Task<ActionResult<ApiResponse<List<Permission>>>> List(string systemName="")
        {

            var query = repository.Query();
            if (!string.IsNullOrEmpty(systemName))
            {
                query = query.Where(t => t.SystemName == systemName.Trim());
            }
            return this.OkResponse(await query.ToListAsync());
        }
        [HttpGet("Detail/{id}")]
        [PermissionKey("Permission.Detail")]
        public async Task<ActionResult<ApiResponse<Permission>>> Detail(long id)
        {
        var permission=   await repository.GetByIdAsync(id);
            return this.OkResponse(permission);
        }

        [HttpGet("Pagination")]
        [PermissionKey("Permission.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Permission>>>> ListByPagination(int pageIndex = 1, int pageSize = 10, string title = "", string permissionkey = "",string systemName="")
        {

            var query = repository.Query();
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(permissionkey))
            {
                query = query.Where(t => !string.IsNullOrEmpty(t.PermissionKey) && t.PermissionKey.Contains(permissionkey.Trim()));

            }
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                query = query.Where(t => !string.IsNullOrEmpty(t.SystemName) && t.SystemName==systemName);
            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }

        [HttpPost]
        [PermissionKey("Permission.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreatePermissionDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByPermissionKeyAsync(dto.PermissionKey,dto.SystemName);
            if (user != null) this.FailResponse("PermissionKey exists");

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);

            return this.OkResponse(info.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Permission.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreatePermissionDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await repository.GetByIdAsync(id);
            if (info == null) this.FailResponse("not found");

            if ((!string.IsNullOrEmpty(dto.PermissionKey) || !string.IsNullOrEmpty(dto.SystemName)) && (dto.PermissionKey != info.PermissionKey || dto.SystemName != info.SystemName))
            {
                if ((await repository.GetByPermissionKeyAsync(dto.PermissionKey,dto.SystemName)) != null)
                {
                    return this.FailResponse(" PermissionKey already exists");
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Permission.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return this.FailResponse(" not found");
            var roles = await repository.GetRolesByPermissionId(id);
            if (roles.Count > 0)
            {
                return this.FailResponse("exists assigned roles");
            }
            info.SoftDelete(currentUser);


            return this.OkResponse(id);
        }




    }


}
