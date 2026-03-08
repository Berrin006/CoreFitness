using Domain.Abstractions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Logging;

public static class LoggingRegistrationExtension
{
    public static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddScoped<ILogger, Logger>();

        return services;
    }
}
