using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoccerOnlineManager.Application.Helpers;
using SoccerOnlineManager.Infrastructure.Contexts;
using SoccerOnlineManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoccerOnlineManager.API.Helpers
{
    public static class DbMigrationHelpers
    {
        public static void EnsureSeedData(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>())
                {
                    context.Database.EnsureCreated();
                    HashHelper.CreatePasswordHash("admin", out var adminHash, out var adminSalt);
                    var admin = new User
                    {
                        Id = Guid.Parse("698a7453-3b94-4cd0-855f-588e1c06dbf5"),
                        Email = "admin@som.com",
                        PasswordHash = adminHash,
                        PasswordSalt = adminSalt
                    };
                    var adminRole = new Role
                    {
                        Id = Guid.Parse("04dc50c8-6544-4374-bab6-2bf45b9f6ad9"),
                        Name = "Admin"
                    };

                    admin.UserRoles = new List<UserRole>();
                    admin.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id });
                    
                    if (!context.Users.Any(u => u.Id == admin.Id)) context.Add(admin);
                    if (!context.Roles.Any(u => u.Id == adminRole.Id)) context.Add(adminRole);

                    context.SaveChanges();
                }
            }
        }
    }
}
