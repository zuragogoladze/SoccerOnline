using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Commands.User;
using SoccerOnlineManager.Application.Queries.User;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.UnitTests
{
    public class UserTests : TestBase
    {
        public UserTests() : base(nameof(UserTests))
        { }

        [Fact]
        public async Task Create_creates_user()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var createUserCommand = new CreateUserCommand { Email = "test1@som.com", Password = "test" };
                await mediatr.Send(createUserCommand);

                // Assert
                Assert.True(context.Users.Any(r => r.Email == createUserCommand.Email));
            }
        }

        [Fact]
        public async Task Update_updates_user()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var userId = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3");
                var targetEmail = "user1changed@som.com";

                // Act
                var updateUserCommand = new UpdateUserCommand(userId, targetEmail);
                await mediatr.Send(updateUserCommand);
                var user = context.Users.Find(userId);

                // Assert
                Assert.Equal(targetEmail, user.Email);
            }
        }

        [Fact]
        public async Task Delete_deletes_user()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var deleteUserCommand = new DeleteUserCommand { Id = userId };
                await mediatr.Send(deleteUserCommand);

                // Assert
                Assert.False(context.Users.Any(u => u.Id == userId));
            }
        }

        [Fact]
        public async Task GetAll_gets_users()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Act
                var getUsersQuery = new GetUsersQuery();
                var result = await mediatr.Send(getUsersQuery);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Users.Any());
            }
        }

        [Fact]
        public async Task Get_gets_user()
        {
            using (var scope = _webApplicationFactory.Services.CreateScope())
            {
                // Arrange
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var getUserQuery = new GetUserQuery { Id = userId };
                var result = await mediatr.Send(getUserQuery);

                // Assert
                Assert.True(result != null);
            }
        }
    }
}
