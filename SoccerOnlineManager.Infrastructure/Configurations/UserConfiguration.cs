using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerOnlineManager.Infrastructure.Entities;

namespace SoccerOnlineManager.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.Email).HasMaxLength(100).IsUnicode();

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User);

            builder.HasOne(u => u.Team)
                .WithOne(t => t.User)
                .HasForeignKey<Team>(t => t.UserId);
        }
    }
}
