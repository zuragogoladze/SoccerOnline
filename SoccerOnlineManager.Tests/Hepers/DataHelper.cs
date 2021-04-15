using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.API;
using SoccerOnlineManager.Application.Helpers;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Entities;
using SoccerOnlineManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoccerOnlineManager.Tests.Hepers
{
    public static class DataHelper
    {
        public static void SeedData(WebApplicationFactory<Startup> webApplicationFactory)
        {
            using (var scope = webApplicationFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                context.SeedData();
            }
        }

        public static void SeedData(this DatabaseContext context)
        { 
            context.Database.EnsureCreated();
            HashHelper.CreatePasswordHash("admin", out var adminHash, out var adminSalt);
            HashHelper.CreatePasswordHash("user1", out var user1Hash, out var user1Salt);
            HashHelper.CreatePasswordHash("user2", out var user2Hash, out var user2Salt);
            HashHelper.CreatePasswordHash("user3", out var user3Hash, out var user3Salt);
            HashHelper.CreatePasswordHash("user4", out var user4Hash, out var user4Salt);

            var admin = new User
            {
                Id = Guid.Parse("78018bd8-0b5d-40f0-812e-6148add07e15"),
                Email = "admin@som.com",
                PasswordHash = adminHash,
                PasswordSalt = adminSalt
            };
            var user1 = new User
            {
                Id = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3"),
                Email = "user1@som.com",
                PasswordHash = user1Hash,
                PasswordSalt = user1Salt
            };
            var user2 = new User
            {
                Id = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e"),
                Email = "user2@som.com",
                PasswordHash = user2Hash,
                PasswordSalt = user2Salt
            };
            var user3 = new User
            {
                Id = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978"),
                Email = "user3@som.com",
                PasswordHash = user3Hash,
                PasswordSalt = user3Salt
            };
            var user4 = new User
            {
                Id = Guid.Parse("1d7229fd-76b7-46c6-8227-ff6865b91f3e"),
                Email = "user4@som.com",
                PasswordHash = user4Hash,
                PasswordSalt = user4Salt
            };
            var team = new Team
            {
                UserId = user2.Id,
                TransferBudget = 5_000_000
            };
            var team2 = new Team
            {
                UserId = user3.Id,
                TransferBudget = 5_000_000
            };
            var team3 = new Team
            {
                UserId = user4.Id,
                TransferBudget = 1_000_000
            };
            var player = new Player
            {
                Id = Guid.Parse("c612ea74-3c55-4ca9-b536-47fcf6ba8f4c"),
                TeamId = team2.UserId,
                MarketValue = 1_000_000,
                Age = 21,
                Position = Position.Attacker
            };
            var player2 = new Player
            {
                Id = Guid.Parse("f3d9a615-cdc2-40c1-93a2-2f204bfa0cbf"),
                TeamId = team2.UserId,
                MarketValue = 1_000_000,
                Age = 22,
                Position = Position.Attacker
            };
            var player3 = new Player
            {
                Id = Guid.Parse("64b7a83e-52b1-4735-822e-2935904ac61a"),
                TeamId = team2.UserId,
                MarketValue = 1_000_000,
                Age = 23,
                Position = Position.Attacker
            };
            var player4 = new Player
            {
                Id = Guid.Parse("b3f0d890-08d3-46e4-9fb6-b0c5315c4166"),
                TeamId = team2.UserId,
                MarketValue = 1_000_000,
                Age = 24,
                Position = Position.Attacker
            };
            var player5 = new Player
            {
                Id = Guid.Parse("e731ab61-9144-4879-9295-019bfecc8fa4"),
                TeamId = team2.UserId,
                MarketValue = 1_500_000,
                Age = 25,
                Position = Position.Defender
            };
            var player6 = new Player
            {
                Id = Guid.Parse("81a8f8dd-f2eb-45b9-b347-68ebb53b771d"),
                TeamId = team2.UserId,
                MarketValue = 1_500_000,
                Age = 26,
                Position = Position.Defender
            };
            var transfer = new Transfer
            {
                Id = Guid.Parse("5ddcc122-85df-478c-b199-f3acb4aad981"),
                PlayerId = player4.Id,
                Price = 1_000_000,
                Status = TransferStatus.Active,
                FromTeam = team2.UserId
            };
            var transfer2 = new Transfer
            {
                Id = Guid.Parse("1112f868-528e-4195-8cee-b94b5516b4e2"),
                PlayerId = player5.Id,
                Price = 1_200_000,
                Status = TransferStatus.Active,
                FromTeam = team2.UserId
            };
            var transfer3 = new Transfer
            {
                Id = Guid.Parse("15a487af-4459-45d2-a27b-c6e5d8a1266a"),
                PlayerId = player6.Id,
                Price = 1_200_000,
                Status = TransferStatus.Active,
                FromTeam = team3.UserId
            };
            var adminRole = new Role
            {
                Id = Guid.Parse("04dc50c8-6544-4374-bab6-2bf45b9f6ad9"),
                Name = "Admin"
            };

            admin.UserRoles = new List<UserRole>();
            admin.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });

            if (!context.Users.Any(u => u.Id == admin.Id)) context.Add(admin);
            if (!context.Users.Any(u => u.Id == user1.Id)) context.Add(user1);
            if (!context.Users.Any(u => u.Id == user2.Id)) context.Add(user2);
            if (!context.Users.Any(u => u.Id == user3.Id)) context.Add(user3);
            if (!context.Users.Any(u => u.Id == user4.Id)) context.Add(user4);
            if (!context.Teams.Any(u => u.UserId == team.UserId)) context.Add(team);
            if (!context.Teams.Any(u => u.UserId == team2.UserId)) context.Add(team2);
            if (!context.Teams.Any(u => u.UserId == team3.UserId)) context.Add(team3);
            if (!context.Players.Any(u => u.Id == player.Id)) context.Add(player);
            if (!context.Players.Any(u => u.Id == player2.Id)) context.Add(player2);
            if (!context.Players.Any(u => u.Id == player3.Id)) context.Add(player3);
            if (!context.Players.Any(u => u.Id == player4.Id)) context.Add(player4);
            if (!context.Players.Any(u => u.Id == player5.Id)) context.Add(player5);
            if (!context.Players.Any(u => u.Id == player6.Id)) context.Add(player6);
            if (!context.Transfers.Any(u => u.Id == transfer.Id)) context.Add(transfer);
            if (!context.Transfers.Any(u => u.Id == transfer2.Id)) context.Add(transfer2);
            if (!context.Transfers.Any(u => u.Id == transfer3.Id)) context.Add(transfer3);
            if (!context.Roles.Any(u => u.Id == adminRole.Id)) context.Add(adminRole);

            context.SaveChanges();
        }
    }
}
