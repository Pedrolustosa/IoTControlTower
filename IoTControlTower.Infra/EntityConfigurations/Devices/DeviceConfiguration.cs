using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Devices;

public class DeviceConfiguration() : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.Property(x => x.Id).HasColumnName("DeviceId");
        builder.HasOne(d => d.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(d => d.UserId)
                .IsRequired();
    }
}
