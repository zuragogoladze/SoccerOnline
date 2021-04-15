using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Infrastructure.Contexts;

namespace SoccerOnlineManager.Tests.Hepers
{
    public class DataContextMocker
    {
        public static DbContextOptions<DatabaseContext> CreateNewContextOptions(string name)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase(name)
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
