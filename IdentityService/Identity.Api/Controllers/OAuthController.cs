using Identity.Api.Contracts.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        [HttpGet("google")]
        public  ActionResult<ApiResponse<string>> GoogleCallBack(string code,string state)
        {
            return this.OkResponse(code);
        }
    }
}
