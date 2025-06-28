using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class CheckPermissionController(PermissionHelper permissionHelper, IValidator<PermissionCheckDto> permissionValidator) : ControllerBase
    {
        [Authorize] // if you don't want to require authentication for this endpoint, you can remove this line
        [HttpPost]
        [PermissionKey("Permission.Check")]
        public async Task<ActionResult<ApiResponse<string>>> CheckPermission(PermissionCheckDto dto)
        {
            await ValidationHelper.ValidateModelAsync(dto, permissionValidator);

            var res = await permissionHelper.CheckPermissionAsync(dto.SystemName, dto.UserId, dto.PermissionKey);
            if (res)
            {

                return Ok(ApiResponse<string>.Ok("success"));
            }
            //Here, I think it is better to return a 200 OK status with a failure message rather than a 403 Forbidden,
            //because the user had the permission to access this endpoint, but not the specific permission they requested.
            return Ok(ApiResponse<string>.Fail("no permission",403));
        }
    }
}
