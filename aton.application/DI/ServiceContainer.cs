using aton.application.Contracts.User;
using aton.application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
