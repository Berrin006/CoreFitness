using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        return services;
    }
}
