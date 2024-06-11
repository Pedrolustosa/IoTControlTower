using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Users;

public class SeedRoleConfiguration(ILogger<SeedRoleConfiguration> logger) : IEntityTypeConfiguration<IdentityRole>
{
    private readonly ILogger<SeedRoleConfiguration> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        try
        {
            _logger.LogInformation("Configuring seed roles");

            builder.HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
            );

            _logger.LogInformation("Seed roles configuration succeeded");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while configuring seed roles");
            throw new Exception("An error occurred while configuring seed roles.", ex);
        }
    }
}
