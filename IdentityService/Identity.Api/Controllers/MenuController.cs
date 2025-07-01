using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Infrastructure.Migrations;
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
    public class MenuController(IValidator<CreateMenuDto> validator, MenuMapper mapper, MenuRepository repository,BaseDbContext baseDbContext) : ControllerBase
    {

        [HttpGet]
        [PermissionKey("Menu.List")]
        public async Task<ActionResult<ApiResponse<List<menu>>>> List(string systemName = "")
        {
            var menus = baseDbContext.Menus;
            if (!string.IsNullOrEmpty(systemName))
            {
                menus.Where(t => t.SystemName == systemName);
            }
            return this.OkResponse(await menus.ToListAsync());
        }

        [HttpGet("ListWithPermission")]
        [PermissionKey("Menu.ListWithPermission")]
        public async Task<ActionResult<ApiResponse<List<menu>>>> ListWithPermission(string systemName = "")
        {
            var menus = baseDbContext.Menus.Include(t=>t.Permissions);
            if (!string.IsNullOrEmpty(systemName))
            {
                menus.Where(t => t.SystemName == systemName);
            }
            return this.OkResponse(await menus.ToListAsync());
        }
        [HttpPost]
        [PermissionKey("Menu.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByPathAsync(dto.Path,dto.SystemName);
            if (user != null) return this.FailResponse("Menu path exists");

            var info = mapper.ToEntity(dto);

            await repository.AddAsync(info);

            return this.OkResponse(info.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Permission.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await repository.GetByIdAsync(id);
            if (info == null) return this.FailResponse("not found");
            

            if ((!string.IsNullOrEmpty(dto.Path) || !string.IsNullOrEmpty(dto.SystemName)) && (dto.Path != info.Path || dto.SystemName != info.SystemName))
            {
                if ((await repository.GetByPathAsync(dto.Path, dto.SystemName)) != null)
                {
                    return this.FailResponse(" Path already exists");
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Menu.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return this.FailResponse(" not found");
            var users = await repository.GetRolesByMenuId(id);
            if (users.Count > 0)
            {
                return this.FailResponse("exists assigned roles");
            }
            repository.Delete(info);


            return this.OkResponse(id);
        }



    }


}
