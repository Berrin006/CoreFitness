using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence;

public class PersistenceInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IHostEnvironment env, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {

        }
        else
        {
            
        }
    }
}
