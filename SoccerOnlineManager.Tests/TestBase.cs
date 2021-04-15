using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoccerOnlineManager.API;
using SoccerOnlineManager.Application.Commands.User;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Tests.Hepers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SoccerOnlineManager.Tests
{
    public class TestBase
    {
        protected readonly WebApplicationFactory<Startup> _webApplicationFactory;
        protected readonly HttpClient _httpClient;

        protected TestBase(string name)
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
                        services.AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseInMemoryDatabase(name);
                        });
                    });
                });

            _httpClient = _webApplicationFactory.CreateClient();
            DataHelper.SeedData(_webApplicationFactory);
        }

        protected async Task Authenticate(string email, string password)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJWTToken(email, password));
        }

        private async Task<string> GetJWTToken(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("Users/authenticate", new AuthenticateUserCommand
            {
                Email = email,
                Password = password
            });

            var deserializedResponse = await response.Content.ReadFromJsonAsync<AuthenticateUserResponse>();

            return deserializedResponse.Token;
        }
    }
}
