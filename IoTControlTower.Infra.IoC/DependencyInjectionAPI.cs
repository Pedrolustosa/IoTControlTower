using System.Data;
using FluentValidation;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IoTControlTower.Infra.Context;
using Microsoft.AspNetCore.Identity;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Infra.Repository;
using Microsoft.AspNetCore.Mvc.Routing;
using IoTControlTower.Domain.Validation;
using Microsoft.Extensions.Configuration;
using IoTControlTower.Application.Mapping;
using IoTControlTower.Application.Service;
using IoTControlTower.Application.DTO.Email;
using IoTControlTower.Application.Validator;
using IoTControlTower.Application.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using IoTControlTower.Application.Users.Commands;
using IoTControlTower.Application.Devices.Commands;
using IoTControlTower.Domain.Interface.UserRepository;
using IoTControlTower.Infra.Repository.UserRepository;
using IoTControlTower.Infra.Repository.DeviceRepository;

namespace IoTControlTower.Infra.IoC;

public static class DependencyInjectionAPI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
    {
        // Context
        var sqlServerConnection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<IoTControlTowerContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(IoTControlTowerContext).Assembly.FullName)));

        services.AddSingleton<IDbConnection>(provider =>
        {
            var connection = new SqlConnection(sqlServerConnection);
            connection.Open();
            return connection;
        });

        // Identity
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IoTControlTowerContext>().AddDefaultTokenProviders();

        // Email configuration
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailDTO>();
        services.AddSingleton(emailConfig);
        services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);

        // FluentValidator
        services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddTransient<IValidator<CreateDeviceCommand>, CreateDeviceCommandValidator>();

        // Mappings
        services.AddAutoMapper(typeof(DomainToDTOProfile));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserEFRepository>();
        services.AddScoped<IDevicesRepository, DevicesEFRepository>();
        services.AddScoped<IDevicesDapperRepository, DevicesDapperRepository>();

        // Registro do UnitOfWork
        services.AddScoped<UnitOfWork>();

        // Serices
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IAuthenticateService, AuthenticateService>();

        // Url
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        services.AddScoped<IUrlHelper>(x => {
            var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = x.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        });

        var myhandlers = AppDomain.CurrentDomain.Load("IotControlTower.Application");
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myhandlers));
        return services;
    }
}
