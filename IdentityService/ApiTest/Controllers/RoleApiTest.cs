using Identity.Api;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace ApiTest.Controllers
{
    public class RoleApiTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient client;

        public RoleApiTest(WebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient();

        }
        public async Task InitializeAsync()
        {

            var loginResponse = await client.PostAsJsonAsync("/api/Login",
                new LoginRequestDto { UserName = "guest", Password = "123456" });

            loginResponse.EnsureSuccessStatusCode();

            var content = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
            var token = content?.Data?.Token.AccessToken ?? throw new Exception("Login failed");

            // set jwt token for subsequent requests
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        public Task DisposeAsync()
        {
            client.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task GetRoles_ShouldReturnOk()
        {
            var response = await client.GetAsync("/api/role");
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
        }
    }
}