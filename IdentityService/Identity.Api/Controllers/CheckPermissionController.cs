using FluentValidation;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckPermissionController(PermissionHelper permissionHelper, IValidator<PermissionCheckDto> permissionValidator) : ControllerBase
    {
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

            return Ok(ApiResponse<string>.Fail("no permission"));
        }
    }
}
