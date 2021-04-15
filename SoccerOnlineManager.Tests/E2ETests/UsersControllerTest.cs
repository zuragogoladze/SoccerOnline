using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.User;
using SoccerOnlineManager.Application.Queries.Team;
using SoccerOnlineManager.Application.Queries.User;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.E2ETests
{
    public class UsersControllerTest : TestBase
    {
        public UsersControllerTest() : base(nameof(UsersControllerTest))
        { }

        [Fact]
        public async Task Create_creates_user_should_return_created()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                // Act
                var createUserCommand = new CreateUserCommand { Email = "test2@som.com", Password = "test" };
                var response = await _httpClient.PostAsJsonAsync("users", createUserCommand);
                var user = context.Users.FirstOrDefault(u => u.Email == createUserCommand.Email);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);
                user.Should().NotBeNull();
                context.Teams.FirstOrDefault(t => t.UserId == user.Id).Should().NotBeNull();
                context.Players.FirstOrDefault(t => t.TeamId == user.Id).Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_gets_users_team_should_return_ok_and_result()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var createUserCommand = new CreateUserCommand { Email = "test3@som.com", Password = "test" };
                await _httpClient.PostAsJsonAsync("users", createUserCommand);
                await Authenticate(createUserCommand.Email, createUserCommand.Password);

                // Act
                var response = await _httpClient.GetAsync("users/me/team");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<GetTeamWithPlayersResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
                responseDeserialized.Players.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Update_updates_user_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                var targetEmail = "user1changed@som.com";
                await Authenticate("user1@som.com", "user1");

                // Act
                var updateUserCommand = new UpdateUserCommand(userId, targetEmail);
                var response = await _httpClient.PutAsJsonAsync($"users/{userId}", updateUserCommand);
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Update_updates_user_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                var targetEmail = "user1changed@som.com";
                await Authenticate("admin@som.com", "admin");

                // Act
                var updateUserCommand = new UpdateUserCommand(userId, targetEmail);
                var response = await _httpClient.PutAsJsonAsync($"users/{userId}", updateUserCommand);
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
                user.Email.Should().Be(targetEmail);
            }
        }

        [Fact]
        public async Task Delete_deletes_user_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                await Authenticate("user1@som.com", "user1");

                // Act
                var response = await _httpClient.DeleteAsync($"users/{userId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Delete_deletes_user_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                await Authenticate("admin@som.com", "admin");

                // Act
                var response = await _httpClient.DeleteAsync($"users/{userId}");
                var user = context.Users.FirstOrDefault(u => u.Id == userId);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
                user.Should().Be(null);
            }
        }

        [Fact]
        public async Task GetAll_gets_users_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");

                // Act
                var response = await _httpClient.GetAsync("users");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task GetAll_gets_users_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");

                // Act
                var response = await _httpClient.GetAsync("users");
                var content = await response.Content.ReadFromJsonAsync<GetUsersResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                content.Users.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_gets_user_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var response = await _httpClient.GetAsync($"users/{userId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Get_gets_user_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var response = await _httpClient.GetAsync($"users/{userId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
