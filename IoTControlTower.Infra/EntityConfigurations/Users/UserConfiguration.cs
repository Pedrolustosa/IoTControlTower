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
            try
            {
                _logger.LogInformation("Configuring user entity");

                builder.Property(u => u.Id).HasColumnName("UserId");

                _logger.LogInformation("User entity configuration succeeded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while configuring user entity");
                throw new Exception("An error occurred while configuring user entity.", ex);
            }
        }
    }
}
