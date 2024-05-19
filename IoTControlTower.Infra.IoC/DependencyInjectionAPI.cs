using IoTControlTower.Application.Interface;
using IoTControlTower.Application.Mapping;
using IoTControlTower.Application.Service;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using IoTControlTower.Infra.Context;
using IoTControlTower.Infra.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoTControlTower.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IoTControlTowerContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(IoTControlTowerContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IoTControlTowerContext>()
                                                      .AddDefaultTokenProviders();

            //Mappings
            services.AddAutoMapper(typeof(DomainToDTOProfile));

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDevicesRepository, DevicesRepository>();

            //Serices
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            return services;
        }
    }
}
