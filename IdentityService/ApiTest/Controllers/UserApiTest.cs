using Identity.Api;
using Identity.Api.Contracts.Dtos.Request;
using Identity.Api.Contracts.Dtos.Response;
using Identity.Domain.Entity;
using Identity.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;

namespace ApiTest.Controllers
{


    public class UserApiTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient client = null!;
        private string token = null!;

        public UserApiTest(WebApplicationFactory<Program> factory)
        {
            // constructor for sync init
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            client = _factory.CreateClient();

            var loginResponse = await client.PostAsJsonAsync("/api/Login",
                new LoginRequestDto { UserName = "guest", Password = "123456" });

            loginResponse.EnsureSuccessStatusCode();

            var content = await loginResponse.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
            token = content?.Data?.Token.AccessToken ?? throw new Exception("Login failed");

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
        public async Task Test_Login()
        {
            var response = await client.PostAsJsonAsync("/api/Login",
                new LoginRequestDto { UserName = "guest", Password = "123456" });

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
        }

        [Fact]
        public async Task Test_GetUserList()
        {
            var response = await client.GetAsync("/api/User");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserResponseDto>>>();
           
            Assert.True(content.Success);
        }

        [Fact]
        public async Task Test_GetUser()
        {
            var response = await client.GetAsync("/api/User/1390662790494031872");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<UserResponseDto>>();

            Assert.True(content.Success);
        }

        [Fact]
        public async Task Test_GetPaginationUser()
        {
            var response = await client.GetAsync("/api/User/Pagination");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<ApiResponse<PaginationResponse<User>>>();

            Assert.True(content.Success);
        }
    }

}
