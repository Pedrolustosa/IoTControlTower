using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Description).IsRequired().HasMaxLength(500);
            builder.Property(d => d.Url).IsRequired();
            builder.Property(d => d.Manufacturer).HasMaxLength(100);

            builder.HasMany(d => d.CommandDescriptions)
                   .WithOne(c => c.Device)
                   .HasForeignKey(c => c.DeviceId);

            builder.HasOne(d => d.User)
                   .WithMany(u => u.Devices)
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
