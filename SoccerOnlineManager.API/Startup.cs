using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoccerOnlineManager.API.Extensions.Authentication;
using SoccerOnlineManager.API.Extensions.Options;
using SoccerOnlineManager.API.Extensions.Swagger;
using SoccerOnlineManager.API.Helpers;
using SoccerOnlineManager.API.Middlewares;
using SoccerOnlineManager.Application;
using SoccerOnlineManager.Infrastructure;

namespace SoccerOnlineManager.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.ConfigureCustomOptions(Configuration);

            services.AddControllers();

            services.AddScoped<IAppService, AppService>();

            services.AddCors();

            services.AddCustomAuthentication(Configuration);

            services.AddCustomSwagger(Configuration);

            services.AddApplicationLayer(Configuration);

            services.AddInfrastructureLayer(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomSwagger();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
