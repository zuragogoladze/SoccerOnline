using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.Infrastructure.Contexts;
using System;

namespace SoccerOnlineManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LocalDb")));
        }
    }
}
