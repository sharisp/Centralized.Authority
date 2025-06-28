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
    public class MenuController(IValidator<CreateMenuDto> validator, MenuMapper mapper, MenuRepository repository) : ControllerBase
    {

        [HttpPost]
        [PermissionKey("Menu.Create")]
        public async Task<ActionResult<ApiResponse<CreateMenuDto>>> Create(CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByPathAsync(dto.Path,dto.SystemName);
            if (user != null) return Ok(ApiResponse<CreateMenuDto>.Fail("Menu path exists", 400));

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);

            return Ok(ApiResponse<CreateMenuDto>.Ok(dto));
        }

        [HttpPut("{id}")]
        [PermissionKey("Permission.Update")]
        public async Task<ActionResult<ApiResponse<CreateMenuDto>>> Update(long id, [FromBody] CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await repository.GetByIdAsync(id);
            if (info == null) return Ok(ApiResponse<CreateMenuDto>.Fail("not found"));

            if ((!string.IsNullOrEmpty(dto.Path) || !string.IsNullOrEmpty(dto.SystemName)) && (dto.Path != info.Path || dto.SystemName != info.SystemName))
            {
                if ((await repository.GetByPathAsync(dto.Path, dto.SystemName)) != null)
                {
                    return Ok(ApiResponse<CreateMenuDto>.Fail(" Path already exists", 400));
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            return Ok(ApiResponse<CreateMenuDto>.Ok(dto));
        }

        [HttpDelete("{id}")]
        [PermissionKey("Menu.Delete")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return Ok(ApiResponse<string>.Fail(" not found"));
            var users = await repository.GetRolesByMenuId(id);
            if (users.Count > 0)
            {
                return Ok(ApiResponse<string>.Fail("exists assigned roles",400));
            }
            repository.Delete(info);


            return Ok(ApiResponse<string>.Ok("delete successfully"));
        }



    }


}
