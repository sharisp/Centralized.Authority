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
using Identity.Api.Contracts.Dtos.Request;
using Identity.Domain.Entity;

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

            var user = await repository.GetByPermissionKeyAsync(dto.PermissionKey);
            if (user != null) return BadRequest(ApiResponse<CreatePermissionDto>.Fail("PermissionKey exists"));

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
            if (info == null) return NotFound(ApiResponse<CreatePermissionDto>.Fail("not found"));

            if (!string.IsNullOrEmpty(dto.PermissionKey) && dto.PermissionKey != info.PermissionKey)
            {
                if ((await repository.GetByPermissionKeyAsync(dto.PermissionKey)) != null)
                {
                    return BadRequest(ApiResponse<CreatePermissionDto>.Fail(" PermissionKey already exists"));
                }
            }
            mapper.UpdateDtoToEntity(dto, info);

            return Ok(ApiResponse<CreatePermissionDto>.Ok(dto));
        }

        [HttpDelete("{id}")]
        [PermissionKey("Permission.Delete")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var info = await repository.GetByIdAsync(id);
            if (info == null) return NotFound(ApiResponse<bool>.Fail(" not found"));
            var users= await repository.GetRolesByPermissionId(id);
            if (users.Count>0)
            {
                return BadRequest(ApiResponse<string>.Fail("exists assigned roles"));
            }
            repository.Delete(info);


            return Ok(ApiResponse<string>.Ok("delete error"));
        }

    }
}
