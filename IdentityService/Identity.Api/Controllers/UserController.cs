using Domain.SharedKernel.Interfaces;
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
using System.Security.Claims;

namespace Identity.Api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(IValidator<CreateUserRequestDto> validator, UserMapper mapper, IUserRepository userRepository, IMediator mediator, PermissionHelper permissionHelper, ICurrentUser currentUser, IRoleRepository roleRepository, IValidator<ChangePwdDto> pwdValidator) : ControllerBase
    {

        [HttpGet]
        [PermissionKey("User.List")]
        public async Task<ActionResult<ApiResponse<List<User>>>> List()
        {

            var query = userRepository.Query();
            var users = await query.ToListAsync();
            return this.OkResponse(users);
        }

        [HttpGet("{id}")]
        [PermissionKey("User.Detail")]
        public async Task<ActionResult<ApiResponse<List<User>>>> ListById(long id)
        {

            var query = userRepository.Query(true);
            var users = await query.FirstOrDefaultAsync(t => t.Id == id);
            return this.OkResponse(users);
        }
        [HttpGet("Pagination")]
        [PermissionKey("User.List")]
        public async Task<ActionResult<ApiResponse<PaginationResponse<User>>>> ListByPagination(int pageIndex = 1, int pageSize = 10, string userName = "", string phoneNumber = "")
        {

            var query = userRepository.Query();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(t => t.UserName.Contains(userName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                query = query.Where(t => t.Phone != null && t.Phone.Number.Contains(phoneNumber.Trim()));

            }
            var res = await query.ToPaginationResponseAsync(pageIndex, pageSize);

            return this.OkResponse(res);
        }


        [HttpPost]
        [PermissionKey("User.Create")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Create(CreateUserRequestDto userDto)
        {
            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUserByNameAsync(userDto.UserName);
            if (user != null) return this.FailResponse("username exists");

            var userinfo = mapper.ToEntity(userDto);
            var password = AppHelper.ReadAppSetting("DefaultPassword");
            userinfo.ChangePassword(password, true);
            if (userDto.RoleIds != null && userDto.RoleIds.Count > 0)
            {
                var roles = await roleRepository.GetRolesByIds(userDto.RoleIds);
                userinfo.AddRoles(roles);
            }


            await userRepository.AddUserAsync(userinfo);
            return this.OkResponse(userinfo.Id);
        }

        [HttpPut("{id}")]
        [PermissionKey("User.Update")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Update(long id, [FromBody] CreateUserRequestDto userDto)
        {

            await ValidationHelper.ValidateModelAsync(userDto, validator);

            var user = await userRepository.GetUserWithRolesByIdAsync(id);
            if (user == null) return this.FailResponse("user not found");

            if (!string.IsNullOrEmpty(userDto.UserName) && userDto.UserName != user.UserName)
            {
                if ((await userRepository.GetUserByNameAsync(userDto.UserName)) != null)
                {
                    return this.FailResponse("user name already exists");
                }
            }
            mapper.UpdateDtoToEntity(userDto, user);


            user.Roles.Clear();
            if (userDto.RoleIds != null && userDto.RoleIds.Count > 0)
            {
                var roles = await roleRepository.GetRolesByIds(userDto.RoleIds);
                user.AddRoles(roles);
            }

            return this.OkResponse(id);
        }


        [HttpPut("ChangePwd")]
        [PermissionKey("User.ChangeOwnPwd")]
        public async Task<ActionResult<ApiResponse<string>>> ChangeOwnPwd([FromBody] ChangePwdDto changePwdDto)
        {

            await ValidationHelper.ValidateModelAsync(changePwdDto, pwdValidator);
            var userId = this.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return this.FailResponse("please login");
            }
            var user = await userRepository.GetUserByIdAsync(Convert.ToInt64(userId));
            if (user == null) return this.FailResponse("user not found");

            if (user.CheckPassword(changePwdDto.OldPassword) == false)
            {

                return this.FailResponse("wrong password");
            }
            user.ChangePassword(changePwdDto.NewPassword);

            return this.OkResponse("success");
        }
        [HttpDelete("{id}")]
        [PermissionKey("User.Delete")]
        public async Task<ActionResult<ApiResponse<BaseResponse>>> Delete(long id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user == null) return this.FailResponse("user not found");

            user.SoftDelete(currentUser);
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
