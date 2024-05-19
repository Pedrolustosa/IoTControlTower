using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Configurations
{
    public class CommandConfiguration : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CommandText).IsRequired().HasMaxLength(500);

            builder.HasMany(c => c.Parameters)
                   .WithOne(p => p.Command)
                   .HasForeignKey(p => p.CommandId);
        }
    }
}
