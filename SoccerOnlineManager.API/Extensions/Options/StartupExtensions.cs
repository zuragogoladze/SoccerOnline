using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Application.Settings;

namespace SoccerOnlineManager.API.Extensions.Options
{
    public static class StartupExtensions
    {
        public static void ConfigureCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // Add JWT Settings
            var jwtSettingsSection = configuration.GetSection("JWT");
            services.Configure<JWTSettings>(jwtSettingsSection);

            // Add Game Settings
            var gameSettingsSection = configuration.GetSection("GameSettings");
            services.Configure<GameSettings>(gameSettingsSection);
        }
    }
}
