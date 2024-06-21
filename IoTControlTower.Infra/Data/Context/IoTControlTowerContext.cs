using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IoTControlTower.Infra.Data.Context;

public class IoTControlTowerContext(DbContextOptions<IoTControlTowerContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IoTControlTowerContext).Assembly);
    }
}
