using Microsoft.EntityFrameworkCore;
using IoTControlTower.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IoTControlTower.Infra.Context
{
    public class IoTControlTowerContext(DbContextOptions<IoTControlTowerContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<CommandDescription> CommandDescriptions { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IoTControlTowerContext).Assembly);
        }
    }
}
