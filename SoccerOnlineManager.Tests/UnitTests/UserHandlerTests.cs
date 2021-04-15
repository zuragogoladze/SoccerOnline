using MediatR;
using Moq;
using SoccerOnlineManager.Application.Commands.Team;
using SoccerOnlineManager.Application.Commands.User;
using SoccerOnlineManager.Application.Queries.User;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Tests.Hepers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SoccerOnlineManager.Tests.UnitTests
{
    public class UserHandlerTests
    {
        [Fact]
        public async Task Create_creates_user()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(UserHandlerTests))))
            {
                // Arrange
                var mediatorMock = new Mock<IMediator>();
                mediatorMock.Setup(m => m.Send(It.IsAny<CreateTeamCommand>(), It.IsAny<CancellationToken>()));
                var handler = new CreateUserCommandHandler(context, mediatorMock.Object);

                // Act
                var createUserCommand = new CreateUserCommand { Email = "test1@som.com", Password = "test" };
                await handler.Handle(createUserCommand, CancellationToken.None);

                // Assert
                Assert.True(context.Users.Any(r => r.Email == createUserCommand.Email));
            }
        }

        [Fact]
        public async Task Update_updates_user()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(UserHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new UpdateUserCommandHandler(context);
                var userId = Guid.Parse("2ed2cc9d-0ffe-47ac-b39f-b22a2f4b52a3");
                var targetEmail = "user1changed@som.com";

                // Act
                var updateUserCommand = new UpdateUserCommand(userId, targetEmail);
                await handler.Handle(updateUserCommand, CancellationToken.None);
                var user = context.Users.Find(userId);

                // Assert
                Assert.Equal(targetEmail, user.Email);
            }
        }

        [Fact]
        public async Task Delete_deletes_user()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(UserHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new DeleteUserCommandHandler(context);
                var userId = Guid.Parse("02741f40-42d1-4a54-b700-e60f285d347e");

                // Act
                var deleteUserCommand = new DeleteUserCommand { Id = userId };
                await handler.Handle(deleteUserCommand, CancellationToken.None);

                // Assert
                Assert.False(context.Users.Any(u => u.Id == userId));
            }
        }

        [Fact]
        public async Task GetAll_gets_users()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(UserHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetUsersQueryHandler(context);

                // Act
                var getUsersQuery = new GetUsersQuery();
                var result = await handler.Handle(getUsersQuery, CancellationToken.None);

                // Assert
                Assert.True(result != null);
                Assert.True(result.Users.Any());
            }
        }

        [Fact]
        public async Task Get_gets_user()
        {
            using (var context = new DatabaseContext(DataContextMocker.CreateNewContextOptions(nameof(UserHandlerTests))))
            {
                // Arrange
                context.SeedData();
                var handler = new GetUserQueryHandler(context);
                var userId = Guid.Parse("4a8d97eb-b5f3-49b7-86b7-ffcb3b0ef978");

                // Act
                var getUserQuery = new GetUserQuery { Id = userId };
                var result = await handler.Handle(getUserQuery, CancellationToken.None);

                // Assert
                Assert.True(result != null);
            }
        }
    }
}
