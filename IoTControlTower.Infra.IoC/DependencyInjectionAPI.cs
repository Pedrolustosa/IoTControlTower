using FluentValidation;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Infra.Context;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface;
using IoTControlTower.Infra.Repository;
using Microsoft.Extensions.Configuration;
using IoTControlTower.Application.Mapping;
using IoTControlTower.Application.Service;
using IoTControlTower.Application.Validator;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.DTO.Device;
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

            //FluentValidator
            services.AddTransient<IValidator<DeviceDTO>, DeviceValidator>();

            //Mappings
            services.AddAutoMapper(typeof(DomainToDTOProfile));

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDevicesRepository, DevicesRepository>();

            //Serices
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            return services;
        }
    }
}
