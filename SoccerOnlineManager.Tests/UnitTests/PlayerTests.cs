using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Queries.Player;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.UnitTests
{
    public class PlayerTests : TestBase
    {
        public PlayerTests() : base(nameof(PlayerTests))
        { }

        [Fact]
        public async Task Create_creates_team_players()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<GameSettings>>();
                var gameSettings = options.Value;
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var createPlayersCommand = new CreatePlayersCommand { TeamId = teamId };
                await mediatr.Send(createPlayersCommand);
                var teamTargetPlayersCount = gameSettings.TeamGoalkeepersCount + gameSettings.TeamDefendersCount +
                                             gameSettings.TeamMidfieldersCount + gameSettings.TeamAttackersCount;

                // Assert
                Assert.Equal(teamTargetPlayersCount, context.Players.Count(r => r.TeamId == teamId));
            }
        }

        [Fact]
        public async Task Update_updates_player()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var updatePlayerCommand = new UpdatePlayerCommand(playerId, "test name", "test last", "test country",
                                                                  null, userId, false);
                await mediatr.Send(updatePlayerCommand);
                var player = context.Players.FirstOrDefault(t => t.Id == playerId);

                // Assert
                Assert.Equal(player.FirstName, updatePlayerCommand.FirstName);
                Assert.Equal(player.LastName, updatePlayerCommand.LastName);
                Assert.Equal(player.Country, updatePlayerCommand.Country);
            }
        }

        [Fact]
        public async Task Delete_deletes_player()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var playerId = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a");

                // Act
                var deletePlayerCommand = new DeletePlayerCommand { Id = playerId };
                await mediatr.Send(deletePlayerCommand);

                // Assert
                Assert.False(context.Players.Any(u => u.Id == playerId));
            }
        }

        [Fact]
        public async Task GetAll_gets_players()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var getPlayersQuery = new GetPlayersQuery();
                var result = await mediatr.Send(getPlayersQuery);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Players.Count() > 0);
            }
        }

        [Fact]
        public async Task Get_gets_player()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var playerId = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf");

                // Act
                var getPlayerQuery = new GetPlayerQuery { Id = playerId };
                var result = await mediatr.Send(getPlayerQuery);

                // Assert
                Assert.True(result != null);
            }
        }
    }
}
