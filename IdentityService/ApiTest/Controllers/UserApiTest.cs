using Identity.Api;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest.Controllers
{
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using Xunit;
    using Identity.Api.Contracts.Dtos.Request;

    public class UserApiTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient client = null!;
        private string token = null!;

        public UserApiTest(WebApplicationFactory<Program> factory)
        {
            // 构造函数只做同步初始化
            _factory = factory;
        }

        // or use constructor for sync init
        public async Task InitializeAsync()
        {
            client = _factory.CreateClient();

            var loginResponse = await client.PostAsJsonAsync("/api/Login",
                new LoginRequestDto { UserName = "guest", Password = "123456" });

            loginResponse.EnsureSuccessStatusCode();

            var content = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
            token = content?.Data?.Token.AccessToken ?? throw new Exception("Login failed");

            // 设置 Authorization Header
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // 测试结束时释放资源
        public Task DisposeAsync()
        {
            client.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Test1_Login()
        {
            var response = await client.PostAsJsonAsync("/api/Login",
                new LoginRequestDto { UserName = "guest", Password = "123456" });

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task Test2_GetUser()
        {
            var response = await client.GetAsync("/api/User");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
        }
    }

}
