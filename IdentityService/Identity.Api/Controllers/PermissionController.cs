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
    public class PermissionController(IValidator<CreatePermissionDto> validator, PermissionMapper mapper, PermissionRepository repository,BaseDbContext baseDbContext, IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [PermissionKey("Permission.List")]
        public async Task<ActionResult<ApiResponse<List<Permission>>>> List(string systemName="")
        {
            var permissions = baseDbContext.Permissions;
            if (!string.IsNullOrEmpty(systemName))
            {
                permissions.Where(t => t.SystemName == systemName);
            }
            return this.OkResponse(await permissions.ToListAsync());
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
            var users = await repository.GetRolesByPermissionId(id);
            if (users.Count > 0)
            {
                return this.FailResponse("exists assigned roles");
            }
            repository.Delete(info);


            return this.OkResponse(id);
        }




    }


}
