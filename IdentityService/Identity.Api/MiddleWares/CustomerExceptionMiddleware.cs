using System.Net;
using System.Text.Json;
using Identity.Api.ViewModels;

namespace Identity.Api.MiddleWares
{


    public class CustomerExceptionMiddleware(RequestDelegate next, ILogger<CustomerExceptionMiddleware> logger)
    {
        private readonly ILogger<CustomerExceptionMiddleware> _logger = logger;

        //主构造函数
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //  httpContext.Response.ContentType = "application/problem+json";

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError("WebApi——error", ex);
                var res = ApiResponse<string>.Fail(ex.Message);

             await httpContext.Response.WriteAsJsonAsync(res); //这个写法存在大小写问题
                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
               // var stream = httpContext.Response.Body;
               // await JsonSerializer.SerializeAsync(stream, res);
            }
        }
    }

}
