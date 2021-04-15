using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoccerOnlineManager.Infrastructure.Entities;

namespace SoccerOnlineManager.Infrastructure.Configurations
{
    public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.Property(a => a.Price).HasPrecision(16, 3);

            builder.HasOne(t => t.Player)
                .WithMany(p => p.Transfers);
        }
    }
}
