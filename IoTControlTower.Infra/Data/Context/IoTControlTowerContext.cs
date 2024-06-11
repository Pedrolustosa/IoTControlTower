using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IoTControlTower.Infra.Data.Context
{
    public class IoTControlTowerContext(DbContextOptions<IoTControlTowerContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(IoTControlTowerContext).Assembly);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while applying entity configurations: {ex.Message}");
                throw;
            }
        }
    }
}
