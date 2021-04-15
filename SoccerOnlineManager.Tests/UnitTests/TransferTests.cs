﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.Transfer;
using SoccerOnlineManager.Application.Queries.Transfer;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.UnitTests
{
    public class TransferTests : TestBase
    {
        public TransferTests() : base(nameof(TransferTests))
        { }

        [Fact]
        public async Task Create_creates_transfer()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");

                // Act
                var createTransferCommand = new CreateTransferCommand(playerId, 1_000_000, false, userId);
                await mediatr.Send(createTransferCommand);
                
                // Assert
                Assert.True(context.Transfers.Any(t => t.PlayerId == playerId));
            }
        }

        [Fact]
        public async Task GetAll_gets_transfers()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var getTransfersQuery = new GetTransfersQuery();
                var result = await mediatr.Send(getTransfersQuery);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Transfers.Count() > 0);
            }
        }

        [Fact]
        public async Task GetAll_gets_transfers_with_filters()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var getTransfersQuery = new GetTransfersQuery { FromValue = 2_000_000 };
                var result = await mediatr.Send(getTransfersQuery);

                // Assert
                Assert.True(result != null);
                Assert.False(result.Transfers.Any());
            }
        }

        [Fact]
        public async Task Buy_buys_player()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var transferId = Guid.Parse("1112f868-528e-4195-8cee-b94b5516b4e2");
                var toTeamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                var transfer = context.Transfers.Find(transferId);
                var fromTeam = context.Teams.Find(transfer.FromTeam);
                var toTeam = context.Teams.FirstOrDefault(t => t.UserId == toTeamId);
                var fromTeamInitialBudget = fromTeam.TransferBudget;
                var toTeamInitialBudget = toTeam.TransferBudget;

                // Act
                var buyPlayerCommand = new BuyPlayerCommand(transferId, toTeamId);
                await mediatr.Send(buyPlayerCommand);

                // Assert
                Assert.Equal(TransferStatus.Sold, transfer.Status);
                Assert.Equal(fromTeamInitialBudget + transfer.Price, fromTeam.TransferBudget);
                Assert.Equal(toTeamInitialBudget - transfer.Price, toTeam.TransferBudget);
            }
        }

        [Fact]
        public async Task Buy_can_not_buy_player()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var transferId = Guid.Parse("15a487af-4459-45d2-a27b-c6e5d8a1266a");
                var toTeamId = Guid.Parse("1d7229fd-76b7-46c6-8227-ff6865b91f3e");
                var transfer = context.Transfers.Find(transferId);
                var fromTeam = context.Teams.Find(transfer.FromTeam);
                var toTeam = context.Teams.FirstOrDefault(t => t.UserId == toTeamId);
                var fromTeamInitialBudget = fromTeam.TransferBudget;
                var toTeamInitialBudget = toTeam.TransferBudget;

                // Act
                var buyPlayerCommand = new BuyPlayerCommand(transferId, toTeamId);
                try
                {
                    await mediatr.Send(buyPlayerCommand);
                }
                catch
                {

                }

                // Assert
                Assert.Equal(TransferStatus.Active, transfer.Status);
                Assert.Equal(fromTeamInitialBudget, fromTeam.TransferBudget);
                Assert.Equal(toTeamInitialBudget, toTeam.TransferBudget);
            }
        }
    }
}
