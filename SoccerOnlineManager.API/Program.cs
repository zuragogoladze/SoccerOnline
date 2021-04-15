using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.Application.Helpers;

namespace SoccerOnlineManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            DbMigrationHelpers.EnsureSeedData(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
