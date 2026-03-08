using Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Fixtures;

public sealed class PersistenceFixture : IAsyncLifetime
{
    private SqliteConnection? _conn;
    public DbContextOptions<DataContext> Options { get; private set; } = default!;
    public DataContext CreateContext() => new(Options);

    public async Task DisposeAsync()
    {
        if (_conn is not null)
        {
            await _conn.CloseAsync();
            await _conn.DisposeAsync();
        }
    }

    public async Task InitializeAsync()
    {
        _conn = new SqliteConnection("Data Source=:memory:;");
        await _conn.OpenAsync();

        Options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(_conn)
            .Options;

        await using var context = new DataContext(Options);
        await context.Database.EnsureCreatedAsync();
    }
}

public sealed class PersistenceCollection : ICollectionFixture<PersistenceFixture>
{
    public const string Name = "Persistence";
}