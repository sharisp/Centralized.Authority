using Identity.Api.Contracts.Dtos.Response;
using Identity.Infrastructure;
using Identity.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OAuthController() : ControllerBase
    {
        [HttpGet("google")]
        public async Task<ActionResult<ApiResponse<string>>> GoogleOAuthCallBack(string state, string code = "", string error = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                return this.FailResponse(error);
            }

            return this.OkResponse("ok");
        }
    }
}
