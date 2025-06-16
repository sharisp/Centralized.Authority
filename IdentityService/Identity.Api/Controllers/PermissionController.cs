using Identity.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ApiAuth.Entities.DTO;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Response;
using FluentValidation;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Interfaces;
using Azure;
using Identity.Domain.Events;
using Identity.Infrastructure.Repository;
using MediatR;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PermissionController(IValidator<CreateRoleDto> validator, RoleMapper mapper, RoleRepository repository, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [PermissionKey("Permission.Create")]
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
        [PermissionKey("Permission.Update")]
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
        [PermissionKey("Permission.Delete")]
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

    }
}
