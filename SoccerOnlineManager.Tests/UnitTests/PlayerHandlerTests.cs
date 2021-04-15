using Microsoft.Extensions.Options;
using Moq;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Queries.Player;
using SoccerOnlineManager.Application.Settings;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Tests.Hepers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.UnitTests
{
    public class PlayerHandlerTests
    {
        [Fact]
        public async Task Create_creates_team_players()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(PlayerHandlerTests))))
            {
                // Arrange
                var optionsMock = new Mock<IOptions<GameSettings>>();
                optionsMock.Setup(x => x.Value).Returns(new GameSettings
                {
                    TeamGoalkeepersCount = 3,
                    TeamDefendersCount = 6,
                    TeamMidfieldersCount = 6,
                    TeamAttackersCount = 5
                });
                var handler = new CreatePlayersCommandHandler(context, optionsMock.Object);
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                var gameSettings = optionsMock.Object.Value;

                // Act
                var createPlayersCommand = new CreatePlayersCommand { TeamId = teamId };
                await handler.Handle(createPlayersCommand, CancellationToken.None);
                var teamTargetPlayersCount = gameSettings.TeamGoalkeepersCount + gameSettings.TeamDefendersCount +
                                             gameSettings.TeamMidfieldersCount + gameSettings.TeamAttackersCount;

                // Assert
                Assert.Equal(teamTargetPlayersCount, context.Players.Count(r => r.TeamId == teamId));
            }
        }

        [Fact]
        public async Task Update_updates_player()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(PlayerHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new UpdatePlayerCommandHandler(context);
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var updatePlayerCommand = new UpdatePlayerCommand(playerId, "test name", "test last", "test country",
                                                                  null, userId, false);
                await handler.Handle(updatePlayerCommand, CancellationToken.None);
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
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(PlayerHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new DeletePlayerCommandHandler(context);
                var playerId = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a");

                // Act
                var deletePlayerCommand = new DeletePlayerCommand { Id = playerId };
                await handler.Handle(deletePlayerCommand, CancellationToken.None);

                // Assert
                Assert.False(context.Players.Any(u => u.Id == playerId));
            }
        }

        [Fact]
        public async Task GetAll_gets_players()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(PlayerHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetPlayersQueryHandler(context);

                // Act
                var getPlayersQuery = new GetPlayersQuery();
                var result = await handler.Handle(getPlayersQuery, CancellationToken.None);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Players.Count() > 0);
            }
        }

        [Fact]
        public async Task Get_gets_player()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(PlayerHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetPlayerQueryHandler(context);
                var playerId = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf");

                // Act
                var getPlayerQuery = new GetPlayerQuery { Id = playerId };
                var result = await handler.Handle(getPlayerQuery, CancellationToken.None);

                // Assert
                Assert.True(result != null);
            }
        }
    }
}
