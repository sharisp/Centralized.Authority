using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Entity;
using Identity.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SysController(SysRepository sysRepository) : ControllerBase
    {
        [HttpGet]
        [PermissionKey("Sys.List")]
        public async Task<ActionResult<ApiResponse<List<Sys>>>> List()
        {
            return this.OkResponse(await sysRepository.GetAllSys());
        }
    }
}
