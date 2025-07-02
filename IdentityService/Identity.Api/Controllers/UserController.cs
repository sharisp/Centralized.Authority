using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.Contracts.Mapping;
using Identity.Domain.Entity;
using Identity.Domain.Events;
using Identity.Domain.Interfaces;
using Identity.Infrastructure;
using Identity.Infrastructure.Extensions;
using Identity.Infrastructure.Options;
using Identity.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(IValidator<CreateUserRequestDto> validator, UserMapper mapper, IUserRepository userRepository, IMediator mediator,BaseDbContext baseDbContext, PermissionHelper permissionHelper, ICurrentUser currentUser) : ControllerBase
    {

        [HttpGet]
        [PermissionKey("User.List")]
        public async Task<ActionResult<ApiResponse<List<User>>>> List()
        {
            var users =await baseDbContext.Users.ToListAsync();
            return this.OkResponse(users);
        }

        [HttpGet("Pagination")]
        [PermissionKey("User.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<User>>>> ListByPagination(int pageIndex = 1, int pageSize = 10, string userName = "", string phoneNumber = "")
        {
            var users = baseDbContext.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                users = users.Where(t => t.UserName.Contains(userName));
            }
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                users = users.Where(t => t.Phone!=null && t.Phone.Number.Contains(phoneNumber));

            }
            var res = await users.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }


        [HttpPost]
        [PermissionKey("User.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreateUserRequestDto userDto)
        {
            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUseByNameAsync(userDto.UserName);
            if (user != null) return  this.FailResponse("username exists");
        
            var userinfo = mapper.ToEntity(userDto);

            await userRepository.AddUserAsync(userinfo);

           // var respDto = mapper.ToResponseDto(userinfo);
            return this.OkResponse(userinfo.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("User.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateUserRequestDto userDto)
        {
            
            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null) return this.FailResponse("user not found");

            if (!string.IsNullOrEmpty(userDto.UserName) && userDto.UserName != user.UserName)
            {
                if ((await userRepository.GetUseByNameAsync(userDto.UserName)) != null)
                {
                    return this.FailResponse("user name already exists");
                }
            }
            mapper.UpdateDtoToEntity(userDto, user);

            // userRepository.UpdateUserAsync(user);
            var respDto = mapper.ToResponseDto(user);
            return this.OkResponse(id);
        }

        [HttpDelete("{id}")]
        [PermissionKey("User.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null) return this.FailResponse("user not found");

            userRepository.DeleteUser(user);

            //await RedisHelper.DelAsync($"user_permissions_{id}");
            await mediator.Publish(new UserDeleteEvents(user));
            return this.OkResponse(id);
        }


        [HttpGet("ListPermissionKeys/{systemName}")]
        [PermissionKey("User.PermissionKeys")]
        public async Task<ActionResult<ApiResponse<string[]>>> ListPermissionKeys(string systemName, long? id = null)
        {
            if (id == null)
            {
                id = Convert.ToInt64(currentUser.UserId);
            }
            var userId = id.Value;
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return this.FailResponse("user not found");

            var list = await permissionHelper.GetPermissionsBySystemNameAndUidAsync(userId, systemName);
            return this.OkResponse(list);
        }


        [HttpGet("ListMenus/{systemName}")]
        [PermissionKey("User.Menus")]
        public async Task<ActionResult<ApiResponse<Menu[]>>> ListMenus(string systemName, long? id = null)
        {
            if (id == null)
            {
                id = Convert.ToInt64(currentUser.UserId);
            }
            var userId = id.Value;
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return this.FailResponse("user not found");

            var list = await permissionHelper.GetMenusBySystemNameAndUid(userId, systemName);
            return this.OkResponse(list);
        }

    }
}
