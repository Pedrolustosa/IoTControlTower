using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.Configurations
{
    public class CommandDescriptionConfiguration : IEntityTypeConfiguration<CommandDescription>
    {
        public void Configure(EntityTypeBuilder<CommandDescription> builder)
        {
            builder.HasKey(cd => cd.Id);
            builder.Property(cd => cd.Operation).IsRequired().HasMaxLength(200);
            builder.Property(cd => cd.Description).HasMaxLength(500);
            builder.Property(cd => cd.Result).HasMaxLength(500);
            builder.Property(cd => cd.Format).HasMaxLength(500);
            builder.Property(cd => cd.DeviceId).IsRequired();
            builder.Property(cd => cd.CommandId).IsRequired();

            builder.HasOne(cd => cd.Device)
                   .WithMany(d => d.CommandDescriptions)
                   .HasForeignKey(cd => cd.DeviceId);

            builder.HasOne(cd => cd.Command)
                   .WithMany(c => c.CommandDescriptions)
                   .HasForeignKey(cd => cd.CommandId);
        }
    }
}
