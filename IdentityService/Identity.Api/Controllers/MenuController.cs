using Domain.SharedKernel.Interfaces;
using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Entity;
using Identity.Infrastructure.Extensions;
using Identity.Infrastructure.Migrations;
using Identity.Infrastructure.Options;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MenuController(IValidator<CreateMenuDto> validator, MenuMapper mapper, MenuRepository repository,  PermissionRepository permissionRepository, ICurrentUser user) : ControllerBase
    {

        [HttpGet]
        [PermissionKey("Menu.List")]
        public async Task<ActionResult<ApiResponse<List<Menu>>>> List(string systemName = "")
        {

            var query = repository.Query();
            if (!string.IsNullOrEmpty(systemName))
            {
                query = query.Where(t => t.SystemName == systemName);
            }
            return this.OkResponse(await query.ToListAsync());
        }
        [HttpGet("Detail/{id}")]
        [PermissionKey("Menu.Detail")]
        public async Task<ActionResult<ApiResponse<Menu>>> Detail(long id)
        {
            var query = repository.Query(true);
            var menu = await query.FirstOrDefaultAsync(t => t.Id == id);
            return this.OkResponse(menu);
        }
        [HttpGet("Pagination")]
        [PermissionKey("Menu.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<Menu>>>> ListByPagination(int pageIndex = 1, int pageSize = 10, string title = "", string permissionkey = "", string systemName = "")
        {

            var query = repository.Query();
         
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(t => t.Title.Contains(title.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(systemName))
            {
                query = query.Where(t => !string.IsNullOrEmpty(t.SystemName) && t.SystemName == systemName);

            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }
        [HttpGet("ListWithPermission")]
        [PermissionKey("Menu.ListWithPermission")]
        public async Task<ActionResult<ApiResponse<List<Menu>>>> ListWithPermission(string systemName = "")
        {
            var query = repository.Query(true);          
            if (!string.IsNullOrEmpty(systemName))
            {
                query= query.Where(t => t.SystemName == systemName);
            }
            return this.OkResponse(await query.ToListAsync());
        }
        [HttpPost]
        [PermissionKey("Menu.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var user = await repository.GetByPathAsync(dto.Path, dto.SystemName);
            if (user != null) return this.FailResponse("Menu path exists");

            var info = mapper.ToEntity(dto);
            if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
            {
                var permissions = await permissionRepository.GetPermissionsByIds(dto.PermissionIds);
                info.AddPermissions(permissions);
            }
            await repository.AddAsync(info);

            return this.OkResponse(info.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("Permission.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateMenuDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, validator);

            var info = await repository.GetByIdWithPermissionAsync(id);
            if (info == null) return this.FailResponse("not found");


            if ((!string.IsNullOrEmpty(dto.Path) || !string.IsNullOrEmpty(dto.SystemName)) && (dto.Path != info.Path || dto.SystemName != info.SystemName))
            {
                if ((await repository.GetByPathAsync(dto.Path, dto.SystemName)) != null)
                {
                    return this.FailResponse(" Path already exists");
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            info.Permissions.Clear();
            if (dto.PermissionIds != null && dto.PermissionIds.Count > 0)
            {
                var permissions = await permissionRepository.GetPermissionsByIds(dto.PermissionIds);
                info.AddPermissions(permissions);
            }
            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("Menu.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var menu = await repository.GetByIdAsync(id);
            if (menu == null) return this.FailResponse(" not found");
            var roles = await repository.GetRolesByMenuId(id);
            if (roles.Count > 0)
            {
                return this.FailResponse("exists assigned roles");
            }
            menu.SoftDelete(user);


            return this.OkResponse(id);
        }



    }


}
