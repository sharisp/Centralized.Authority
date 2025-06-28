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
    public class PermissionController(IValidator<CreatePermissionDto> validator, PermissionMapper mapper, PermissionRepository repository, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [PermissionKey("Permission.Create")]
        public async Task<ActionResult<ApiResponse<CreatePermissionDto>>> Create(CreatePermissionDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByPermissionKeyAsync(dto.PermissionKey,dto.SystemName);
            if (user != null) return Ok(ApiResponse<CreatePermissionDto>.Fail("PermissionKey exists",400));

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);

            return Ok(ApiResponse<CreatePermissionDto>.Ok(dto));
        }

        [HttpPut("{id}")]
        [PermissionKey("Permission.Update")]
        public async Task<ActionResult<ApiResponse<CreatePermissionDto>>> Update(long id, [FromBody] CreatePermissionDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await repository.GetByIdAsync(id);
            if (info == null) return Ok(ApiResponse<CreatePermissionDto>.Fail("not found"));

            if ((!string.IsNullOrEmpty(dto.PermissionKey) || !string.IsNullOrEmpty(dto.SystemName)) && (dto.PermissionKey != info.PermissionKey || dto.SystemName != info.SystemName))
            {
                if ((await repository.GetByPermissionKeyAsync(dto.PermissionKey,dto.SystemName)) != null)
                {
                    return Ok(ApiResponse<CreatePermissionDto>.Fail(" PermissionKey already exists",400));
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            return Ok(ApiResponse<CreatePermissionDto>.Ok(dto));
        }

        [HttpDelete("{id}")]
        [PermissionKey("Permission.Delete")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return Ok(ApiResponse<string>.Fail(" not found"));
            var users = await repository.GetRolesByPermissionId(id);
            if (users.Count > 0)
            {
                return Ok(ApiResponse<string>.Fail("exists assigned roles",400));
            }
            repository.Delete(info);


            return Ok(ApiResponse<string>.Ok("delete successfully"));
        }



    }


}
