using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerOnlineManager.Infrastructure.Entities;

namespace SoccerOnlineManager.Infrastructure.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(200);
            builder.Property(a => a.Country).HasMaxLength(100);
            builder.Property(a => a.TransferBudget).HasPrecision(16, 3);

            builder.HasMany(t => t.Players)
                .WithOne(p => p.Team);
        }
    }
}
