using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.Team;
using SoccerOnlineManager.Application.Queries.Team;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.E2ETests
{
    public class TeamsControllerTest : TestBase
    {
        public TeamsControllerTest() : base(nameof(TeamsControllerTest))
        { }

        [Fact]
        public async Task Update_user_updates_others_team_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var updateTeamCommand = new UpdateTeamCommand("test team", "test country", null, teamId);
                var response = await _httpClient.PutAsJsonAsync($"teams/{teamId}", updateTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Update_admin_updates_others_team_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var updateTeamCommand = new UpdateTeamCommand("test team", "test country", null, teamId);
                var response = await _httpClient.PutAsJsonAsync($"teams/{teamId}", updateTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task Update_user_updates_her_team_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var updateTeamCommand = new UpdateTeamCommand("test team", "test country", null, teamId);
                var response = await _httpClient.PutAsJsonAsync($"teams/{teamId}", updateTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task Create_creates_team_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var teamOwnerId = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3");

                // Act
                var createTeamCommand = new CreateTeamCommand { UserId = teamOwnerId };
                var response = await _httpClient.PostAsJsonAsync("teams", createTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Create_creates_team_should_return_created()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var teamOwnerId = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3");

                // Act
                var createTeamCommand = new CreateTeamCommand { UserId = teamOwnerId };
                var response = await _httpClient.PostAsJsonAsync("teams", createTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task GetAll_gets_teams_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");

                // Act
                var response = await _httpClient.GetAsync("teams");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task GetAll_gets_teams_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");

                // Act
                var response = await _httpClient.GetAsync("teams");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<GetTeamsResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
                responseDeserialized.Teams.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_gets_team_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var response = await _httpClient.GetAsync($"teams/{teamId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Get_gets_team_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var response = await _httpClient.GetAsync($"teams/{teamId}");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<TeamDTO>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Delete_deletes_team_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");
                await Authenticate("user1@som.com", "user1");

                // Act
                var response = await _httpClient.DeleteAsync($"users/{teamId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Delete_deletes_team_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");
                await Authenticate("admin@som.com", "admin");

                // Act
                var response = await _httpClient.DeleteAsync($"users/{teamId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
    }
}
