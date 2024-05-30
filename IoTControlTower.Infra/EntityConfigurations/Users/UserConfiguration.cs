using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IoTControlTower.Infra.EntityConfigurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(d => d.Id);
    }
}
