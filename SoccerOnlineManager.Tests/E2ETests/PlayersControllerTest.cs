using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Queries.Player;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.E2ETests
{
    public class PlayersControllerTest : TestBase
    {
        public PlayersControllerTest() : base(nameof(PlayersControllerTest))
        { }

        [Fact]
        public async Task Update_user_updates_others_team_player_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user2@som.com", "user2");
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");

                // Act
                var updatePlayerCommand = new UpdatePlayerCommand(playerId, "test firs", "test last", "test country",
                                                                  null, Guid.Empty, true);

                var response = await _httpClient.PutAsJsonAsync($"players/{playerId}", updatePlayerCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Update_admin_updates_others_team_player_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");

                // Act
                var updatePlayerCommand = new UpdatePlayerCommand(playerId, "test firs", "test last", "test country",
                                                                  null, Guid.Empty, true);

                var response = await _httpClient.PutAsJsonAsync($"players/{playerId}", updatePlayerCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task Update_user_updates_her_team_player_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");

                // Act
                var updatePlayerCommand = new UpdatePlayerCommand(playerId, "test firs", "test last", "test country",
                                                                  null, Guid.Empty, true);

                var response = await _httpClient.PutAsJsonAsync($"players/{playerId}", updatePlayerCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task Create_creates_player_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var createTeamCommand = new CreatePlayerCommand
                {
                    TeamId = teamId,
                    Age = 23,
                    Country = "Georgia",
                    FirstName = "test first",
                    LastName = "test last",
                    MarketValue = 1_000_000,
                    Position = Position.Attacker
                };
                var response = await _httpClient.PostAsJsonAsync("players", createTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Create_creates_player_should_return_created()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var createTeamCommand = new CreatePlayerCommand
                {
                    TeamId = teamId,
                    Age = 23,
                    Country = "Georgia",
                    FirstName = "test first",
                    LastName = "test last",
                    MarketValue = 1_000_000,
                    Position = Position.Attacker
                };
                var response = await _httpClient.PostAsJsonAsync("players", createTeamCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task GetAll_gets_players_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");

                // Act
                var response = await _httpClient.GetAsync("players");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task GetAll_gets_players_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");

                // Act
                var response = await _httpClient.GetAsync("players");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<GetPlayersResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
                responseDeserialized.Players.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_gets_player_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");
                var playerId = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf");

                // Act
                var response = await _httpClient.GetAsync($"players/{playerId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Get_gets_player_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var playerId = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf");

                // Act
                var response = await _httpClient.GetAsync($"players/{playerId}");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<PlayerDTO>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Delete_deletes_player_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user1@som.com", "user1");
                var playerId = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a");

                // Act
                var response = await _httpClient.DeleteAsync($"players/{playerId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Delete_deletes_player_should_return_no_content()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var playerId = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a");

                // Act
                var response = await _httpClient.DeleteAsync($"players/{playerId}");

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }
    }
}
