using Identity.Api.Contracts.Dtos.Response;
using System.Net;

namespace Identity.Api.MiddleWares
{

    /// <summary>
    /// global exception middleware
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public class CustomerExceptionMiddleware(RequestDelegate next, ILogger<CustomerExceptionMiddleware> logger)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                //  httpContext.Response.ContentType = "application/problem+json";
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
              //  logger.LogError("WebApi——error", ex);
                var res = ApiResponse<string>.Fail(ex.Message);

             await httpContext.Response.WriteAsJsonAsync(res); //这个写法存在大小写问题
                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
               // var stream = httpContext.Response.Body;
               // await JsonSerializer.SerializeAsync(stream, res);
            }
        }
    }

}
