using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.Transfer;
using SoccerOnlineManager.Application.Queries.Transfer;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.E2ETests
{
    public class TransfersControllerTest : TestBase
    {
        public TransfersControllerTest() : base(nameof(TransfersControllerTest))
        { }

        [Fact]
        public async Task Create_user_adds_others_team_player_to_transfers_should_return_forbidden()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user2@som.com", "user2");
                var playerId = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c");

                // Act
                var createTransferCommand = new CreateTransferCommand(playerId, 1_000_000, false, Guid.Empty);

                var response = await _httpClient.PostAsJsonAsync($"transfers", createTransferCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            }
        }

        [Fact]
        public async Task Create_admin_adds_others_team_player_to_transfers_should_return_created()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("admin@som.com", "admin");
                var playerId = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf");

                // Act
                var createTransferCommand = new CreateTransferCommand(playerId, 1_000_000, false, Guid.Empty);

                var response = await _httpClient.PostAsJsonAsync($"transfers", createTransferCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task Create_user_adds_her_team_player_to_transfers_should_return_created()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var playerId = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a");

                // Act
                var createTransferCommand = new CreateTransferCommand(playerId, 1_000_000, false, Guid.Empty);

                var response = await _httpClient.PostAsJsonAsync($"transfers", createTransferCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task Create_user_adds_team_player_twice_to_transfers_should_return_bad_request()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user3@som.com", "user3");
                var playerId = Guid.Parse("b3f0d890-08d3-46e4-9fb6-b0c5315c4166");

                // Act
                var createTransferCommand = new CreateTransferCommand(playerId, 1_000_000, false, Guid.Empty);

                var response = await _httpClient.PostAsJsonAsync($"transfers", createTransferCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact]
        public async Task Get_gets_transfers_should_return_ok_and_result()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user2@som.com", "user2");

                // Act
                var response = await _httpClient.GetAsync("transfers");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<GetTransfersResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
                responseDeserialized.Transfers.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_gets_transfers_with_filters_shoult_return_empty_result()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await Authenticate("user2@som.com", "user2");

                // Act
                var response = await _httpClient.GetAsync("transfers?FromValue=2000000");
                var responseDeserialized = await response.Content.ReadFromJsonAsync<GetTransfersResponse>();

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                responseDeserialized.Should().NotBeNull();
                responseDeserialized.Transfers.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task Buy_user_buys_player_from_transfer_should_return_ok()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var transferId = Guid.Parse("1112f868-528e-4195-8cee-b94b5516b4e2");
                var toTeamId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");
                await Authenticate("user2@som.com", "user2");

                // Act

                var buyPlayerCommand = new BuyPlayerCommand(transferId, toTeamId);
                var response = await _httpClient.PostAsJsonAsync($"transfers/{transferId}/buy", buyPlayerCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Buy_user_buys_player_from_transfer_should_return_bad_request()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var transferId = Guid.Parse("15a487af-4459-45d2-a27b-c6e5d8a1266a");
                var toTeamId = Guid.Parse("1d7229fd-76b7-46c6-8227-ff6865b91f3e");
                await Authenticate("user4@som.com", "user4");

                // Act
                var buyPlayerCommand = new BuyPlayerCommand(transferId, toTeamId);
                var response = await _httpClient.PostAsJsonAsync($"transfers/{transferId}/buy", buyPlayerCommand);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
