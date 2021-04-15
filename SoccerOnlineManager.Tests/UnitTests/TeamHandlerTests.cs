using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using SoccerOnlineManager.Application.Commands.Player;
using SoccerOnlineManager.Application.Commands.Team;
using SoccerOnlineManager.Application.Queries.Team;
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
    public class TeamHandlerTests
    {
        [Fact]
        public async Task Create_creates_team()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(TeamHandlerTests))))
            {
                // Arrange
                var optionsMock = new Mock<IOptions<GameSettings>>();
                var mediatorMock = new Mock<IMediator>();
                optionsMock.Setup(x => x.Value).Returns(new GameSettings
                {
                    TeamInitialTransferBudget = 5000000
                });
                mediatorMock.Setup(m => m.Send(It.IsAny<CreatePlayersCommand>(), It.IsAny<CancellationToken>()));
                var handler = new CreateTeamCommandHandler(context, optionsMock.Object, mediatorMock.Object);
                var teamOwnerId = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3");

                // Act
                var createTeamCommand = new CreateTeamCommand { UserId = teamOwnerId };
                await handler.Handle(createTeamCommand, CancellationToken.None);

                // Assert
                Assert.True(context.Teams.Any(r => r.UserId == teamOwnerId));
            }
        }

        [Fact]
        public async Task Get_gets_team_with_players()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(TeamHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetTeamWithPlayersQueryHandler(context);
                var teamId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");
                var dbTeamValue = context.Teams
                    .Include(t => t.Players)
                    .FirstOrDefault(t => t.UserId == teamId)
                        .Players
                        .Sum(t => t.MarketValue);

                // Act
                var query = new GetTeamWithPlayersQuery { TeamId = teamId };
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.True(result != null);
                Assert.Equal(result.TeamValue, dbTeamValue);
            }
        }

        [Fact]
        public async Task Update_updates_team()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(TeamHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new UpdateTeamCommandHandler(context);
                var teamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var updateTeamCommand = new UpdateTeamCommand("test name", "test country", null, teamId);
                await handler.Handle(updateTeamCommand, CancellationToken.None);
                var team = context.Teams.FirstOrDefault(t => t.UserId == teamId);

                // Assert
                Assert.Equal(team.Name, updateTeamCommand.Name);
                Assert.Equal(team.Country, updateTeamCommand.Country);
            }
        }

        [Fact]
        public async Task GetAll_gets_teams()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(TeamHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetTeamsQueryHandler(context);

                // Act
                var query = new GetTeamsQuery();
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Teams.Count() > 0);
            }
        }

        [Fact]
        public async Task Delete_deletes_team()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(TeamHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new DeleteTeamCommandHandler(context);
                var teamId = Guid.Parse("1d7229fd-76b7-46c6-8227-ff6865b91f3e");

                // Act
                var deleteUserCommand = new DeleteTeamCommand { Id = teamId };
                await handler.Handle(deleteUserCommand, CancellationToken.None);

                // Assert
                Assert.False(context.Teams.Any(u => u.UserId == teamId));
            }
        }
    }
}
