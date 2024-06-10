using System.Data;
using FluentValidation;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using IoTControlTower.Application.CQRS.Devices.Commands;
using IoTControlTower.Application.CQRS.Devices.Validators;
using IoTControlTower.Application.CQRS.Users.Commands;
using IoTControlTower.Application.Mapping;
using IoTControlTower.Application.Interface;
using IoTControlTower.Application.Service;
using IoTControlTower.Application.DTO.Email;
using IoTControlTower.Application.Validator;
using IoTControlTower.Domain.Entities;
using IoTControlTower.Domain.Interface.CachingRepository;
using IoTControlTower.Domain.Interface.UnitOfWork;
using IoTControlTower.Domain.Interface.UserRepository;
using IoTControlTower.Infra.Data.Context;
using IoTControlTower.Infra.Data.Repositories;
using IoTControlTower.Infra.Data.Repositories.CachingRepository;
using IoTControlTower.Infra.Data.Repositories.DeviceRepository;
using IoTControlTower.Infra.Data.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoTControlTower.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            // Context
            var sqlServerConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<IoTControlTowerContext>(options => options.UseSqlServer(sqlServerConnection,
                b => b.MigrationsAssembly(typeof(IoTControlTowerContext).Assembly.FullName)));
            services.AddSingleton<IDbConnection>(provider =>
            {
                var connection = new SqlConnection(sqlServerConnection);
                connection.Open();
                return connection;
            });

            // Identity
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<IoTControlTowerContext>()
                    .AddDefaultTokenProviders();

            // Email configuration
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailDTO>();
            services.AddSingleton(emailConfig);
            services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);

            // FluentValidator
            services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            services.AddTransient<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
            services.AddTransient<IValidator<CreateDeviceCommand>, CreateDeviceCommandValidator>();
            services.AddTransient<IValidator<UpdateDeviceCommand>, UpdateDeviceCommandValidator>();

            // Mappings
            services.AddAutoMapper(typeof(UserProfile), typeof(DeviceProfile));

            // Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserEFRepository>();
            services.AddScoped<IUserDapperRepository, UserDapperRepository>();
            services.AddScoped<IDeviceRepository, DeviceEFRepository>();
            services.AddScoped<IDeviceDapperRepository, DeviceDapperRepository>();

            // UnitOfWork Registration
            services.AddScoped<UnitOfWork>();

            // Cache - Redis
            services.AddScoped<ICachingRepository, CachingRepository>();
            services.AddStackExchangeRedisCache(r =>
            {
                r.InstanceName = "";
                r.Configuration = "localhost:6379";
            });

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();

            // URL
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            // MediatR Handlers
            var myHandlers = AppDomain.CurrentDomain.Load("IotControlTower.Application");
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(myHandlers));

            // MVC
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddRouting(options => { options.LowercaseUrls = true; });

            return services;
        }
    }
}
