using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerOnlineManager.Infrastructure.Entities;

namespace SoccerOnlineManager.Infrastructure.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(a => a.FirstName).HasMaxLength(50);
            builder.Property(a => a.LastName).HasMaxLength(50);
            builder.Property(a => a.Country).HasMaxLength(100);
            builder.Property(a => a.MarketValue).HasPrecision(16, 3);

            builder.HasOne(p => p.Team)
                .WithMany(t => t.Players);

            builder.HasMany(p => p.Transfers)
                .WithOne(t => t.Player);
        }
    }
}
