using Identity.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Response;
using FluentValidation;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Interfaces;
using Azure;
using Identity.Domain.Events;
using MediatR;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Infrastructure;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(IValidator<CreateUserRequestDto> validator, UserMapper mapper, IUserRepository userRepository, IMediator mediator) : ControllerBase
    {

        [HttpPost]
        [PermissionKey("User.Create")]
        public async Task<ActionResult<ApiResponse<CreateUserRequestDto>>> Create(CreateUserRequestDto userDto)
        {
            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUseByNameAsync(userDto.UserName);
            if (user != null) return BadRequest(ApiResponse<CreateUserRequestDto>.Fail("username exists"));

            var userinfo = mapper.ToEntity(userDto);

            await userRepository.AddUserAsync(userinfo);

            var respDto = mapper.ToResponseDto(userinfo);
            return Ok(ApiResponse<UserResponseDto>.Ok(respDto));
        }

        [HttpPut("{id}")]
        [PermissionKey("User.Update")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> Update(long id, [FromBody] CreateUserRequestDto userDto)
        {
            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound(ApiResponse<UserResponseDto>.Fail("user not found"));

            if (!string.IsNullOrEmpty(userDto.UserName) && userDto.UserName != user.UserName)
            {
                if ((await userRepository.GetUseByNameAsync(userDto.UserName)) != null)
                {
                    return BadRequest(ApiResponse<UserResponseDto>.Fail("user name already exists"));
                }
            }
            mapper.UpdateDtoToEntity(userDto, user);

            // userRepository.UpdateUserAsync(user);
            var respDto = mapper.ToResponseDto(user);
            return Ok(ApiResponse<UserResponseDto>.Ok(respDto));
        }

        [HttpDelete("{id}")]
        [PermissionKey("User.Delete")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound(ApiResponse<bool>.Fail("user not found"));

            userRepository.DeleteUser(user);

            //await RedisHelper.DelAsync($"user_permissions_{id}");
            await mediator.Publish(new UserDeleteEvents(user));
            return Ok(ApiResponse<string>.Ok("create error"));
        }

      

    }
}
