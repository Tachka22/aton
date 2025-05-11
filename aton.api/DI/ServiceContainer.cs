using aton.application.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace aton.api.DI;

public static class ServiceContainer
{
    public static void AddSwaggerApi(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header с использованием Bearer схемы. Пример: \"Bearer {токен}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT" 
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        });
    }
    public static void AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Jwt>(configuration.GetSection("Jwt"));
    }
}