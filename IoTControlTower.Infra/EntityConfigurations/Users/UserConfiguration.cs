using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Users
{
    public class UserConfiguration(ILogger<UserConfiguration> logger) : IEntityTypeConfiguration<User>
    {
        private readonly ILogger<UserConfiguration> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public void Configure(EntityTypeBuilder<User> builder)
        {
            _logger.LogInformation("Configuring user entity");

            builder.Property(u => u.Id).HasColumnName("UserId");

            _logger.LogInformation("User entity configuration succeeded");
        }
    }
}
