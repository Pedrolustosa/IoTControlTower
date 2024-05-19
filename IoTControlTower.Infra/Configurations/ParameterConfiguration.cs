using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;

namespace IoTControlTower.Infra.Configurations
{
    public class ParameterConfiguration : IEntityTypeConfiguration<Parameter>
    {
        public void Configure(EntityTypeBuilder<Parameter> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.CommandId).IsRequired();

            builder.HasOne(p => p.Command)
                   .WithMany(c => c.Parameters)
                   .HasForeignKey(p => p.CommandId);
        }
    }
}
