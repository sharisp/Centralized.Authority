using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.MiddleWares;
using Identity.Domain.Constants;
using Identity.Domain.Entity;
using Identity.Infrastructure;
using Identity.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiAuth.Api.Middles;
//dbcontext 不能注入，否则报错
//你不能在中间件构造函数或任何单例生命周期中直接使用 DbContext。推荐使用 IServiceScopeFactory 创建作用域，并在作用域内解析 MyDbContext。

public class CustomJwtAuthMiddleware(
    RequestDelegate next,
    ILogger<CustomerExceptionMiddleware> logger,
    IServiceScopeFactory serviceScopeFactory)
{
    private readonly ILogger<CustomerExceptionMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            //判断是不是白名单的
            var questUrl = context?.Request.Path.Value.ToLower();
            if (string.IsNullOrEmpty(questUrl)) return;
            //白名单验证
            if (CheckWhiteList(questUrl))
            {
                await _next.Invoke(context);
                return;
            }

            // 从 token 中解析用户 ID
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var res = ApiResponse<string>.Fail("no login");

                await context.Response.WriteAsJsonAsync(res);
                //  return;
                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                //  var stream = context.Response.Body;
                //await JsonSerializer.SerializeAsync(stream, res);
                return;
            }

            var permissionKey = context.GetEndpoint()?.Metadata
                .GetMetadata<PermissionKeyAttribute>()?.Key;
            //没有 permissionKey，直接放行
            if (permissionKey == null)
            {
                await _next(context);
                return;
            }
            //can not use DI here
            var permissionHelper = context.RequestServices.GetRequiredService<PermissionHelper>();

            var userIDInt = Convert.ToInt64(userId);
            var resFlag = await permissionHelper.CheckPermissionAsync(Constant.SystemName, userIDInt, permissionKey);

            if (resFlag == false)
            {
                //not contains, no permission
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                var response = ApiResponse<string>.Fail("no auth to this action");

                await context.Response.WriteAsJsonAsync(response);

                return;
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            //  httpContext.Response.ContentType = "application/problem+json";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _logger.LogError("WebApi——异常", ex);
            var res = ApiResponse<string>.Fail(ex.Message);

            await context.Response.WriteAsJsonAsync(res);

        }
    }

    private bool CheckWhiteList(string url)
    {
        var whiteList = AppHelper.ReadAppSettingsSection<List<string>>("WhiteList");

        if (whiteList.Count == 0) return false;

        foreach (var urlitem in whiteList)
            if (urlitem.Trim('/').Equals(url.Trim('/'), StringComparison.OrdinalIgnoreCase))
                return true;
        /*  if (Urlitem.Url.IndexOf("****") > 0)
              {
                  string UrlitemP = Urlitem.url.Replace("****", "");
                  if (Regex.IsMatch(url, UrlitemP, RegexOptions.IgnoreCase)) return true;
                  if (url.Length >= UrlitemP.Length && UrlitemP.ToLower() == url.Substring(0, UrlitemP.Length).ToLower()) return true;

              }*/
        return false;
    }
}