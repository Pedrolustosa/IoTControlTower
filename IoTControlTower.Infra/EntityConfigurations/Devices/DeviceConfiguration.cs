using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Devices;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(d => d.Id);
    }
}
