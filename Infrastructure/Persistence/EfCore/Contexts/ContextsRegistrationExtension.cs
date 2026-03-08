using Domain.Abstractions.Logging;
using Domain.Exceptions.Custom;
using Infrastructure.Logging;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence.EfCore.Contexts;

public static class ContextsRegistrationExtension
{
    public static IServiceCollection AddEfCoreContexts(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(env);

        if (env.IsDevelopment())
        {
            services.AddSingleton<SqliteConnection>(_ =>
            {
                var conn = new SqliteConnection("Data Source=:memory:;");
                conn.Open();

                return conn;
            });

            services.AddDbContext<DataContext>((sp, options) =>
            {
                var conn = sp.GetRequiredService<SqliteConnection>();
                options.UseSqlite(conn);
            });
        }
        else
        {
            services.AddDbContext<DataContext>((sp, options) =>
            {
                ILogger logger = new Logger();

                try
                {
                    var conn = configuration.GetConnectionString("ProductionDatabase")
                        ?? throw new NotFoundDomainException("Production Database ConnectionString Not Found.");

                    options.UseSqlServer(conn);
                }
                catch (Exception ex)
                {
                    logger.Log(ex);
                    throw;
                }
            });
        }

        return services;
    }
}