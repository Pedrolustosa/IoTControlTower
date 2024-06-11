using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Devices;

public class DeviceConfiguration(ILogger<DeviceConfiguration> logger) : IEntityTypeConfiguration<Device>
{
    private readonly ILogger<DeviceConfiguration> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void Configure(EntityTypeBuilder<Device> builder)
    {
        try
        {
            _logger.LogInformation("Configuring Device entity");

            builder.Property(x => x.Id).HasColumnName("DeviceId");
            builder.HasOne(d => d.User)
                  .WithMany(u => u.Devices)
                  .HasForeignKey(d => d.UserId)
                  .IsRequired();

            _logger.LogInformation("Device entity configuration succeeded");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while configuring Device entity");
            throw new Exception("An error occurred while configuring Device entity.", ex);
        }
    }
}
