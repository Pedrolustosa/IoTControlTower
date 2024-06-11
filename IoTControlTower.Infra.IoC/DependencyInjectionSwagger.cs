using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoTControlTower.Infra.IoC;

public static class DependencyInjectionSwagger
{
    public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IoTControlTower.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Pedro Lustosa",
                        Email = "pedroeternalss@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/pedrolustosaengineer/")
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = @"JWT Authorization header using Bearer.
                                        Enter 'Bearer' [space] then put in your token.
                                        Example: 'Bearer 12345abcdef'",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while configuring Swagger: {ex.Message}");
            throw;
        }

        return services;
    }
}
