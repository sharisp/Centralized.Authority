using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Identity.Api.Attributes;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Api.MiddleWares;
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

                await context.Response.WriteAsJsonAsync(res); //这个写法存在大小写问题
                                                              //  return;
                                                              //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                                                              //  var stream = context.Response.Body;
                                                              //await JsonSerializer.SerializeAsync(stream, res);
                return;
            }

            var pemissionKey = context.GetEndpoint()?.Metadata
                .GetMetadata<PermissionKeyAttribute>()?.Key;
            //没有 permissionkey，直接放行
            if (pemissionKey == null)
            {
                await _next(context);
                return;
            }
            // 获取当前请求的路径,不用url ，获取permissionkey
            //   var path = context.Request.Path.Value?.ToLower();

            // 从数据库加载该用户的权限
            var permissionArr = await RedisHelper.LRangeAsync($"user_permissions_{userId}", 0, -1);

            if (permissionArr == null || permissionArr.Length == 0)
                //查询并写入，需要用分布式锁，防止重复写
                await LockHelper.RedisLock($"lock:user:permission:{userId}", async () =>
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

                    var userIDInt = Convert.ToInt64(userId);



                    var permissions = await dbContext.Users.Where(u => u.Id == userIDInt)
                        .SelectMany(u => u.Roles)
                        .SelectMany(r => r.Permissions)
                        .Distinct()
                        .Select(t => t.PermissionKey)
                        .ToListAsync();

                    ;
                    //  foreach (var item in list)
                    foreach (var permission in permissions)
                    {

                        await RedisHelper.RPushAsync($"user_permissions_{userId}", permission);
                    }
                    permissionArr = permissions.ToArray();
                });

            if (permissionArr.Any(x => x.Equals(pemissionKey, StringComparison.OrdinalIgnoreCase)) == false)
            {
                //not contains, no permission
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                var res = ApiResponse<string>.Fail("no auth to this action");

                // await httpContext.Response.WriteAsJsonAsync(res); //这个写法存在大小写问题
                //Serialize the problem details object to the Response as JSON (using System.Text.Json)
                var stream = context.Response.Body;
                await JsonSerializer.SerializeAsync(stream, res);
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

            await context.Response.WriteAsJsonAsync(res); //这个写法存在大小写问题

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