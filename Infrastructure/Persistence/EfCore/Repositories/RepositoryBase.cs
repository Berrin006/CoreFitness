using Domain.Abstractions.Logging;
using Domain.Abstractions.Repositories;
using Domain.Exceptions.Custom;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Repositories;

public abstract class RepositoryBase<TModel, TId, IEntity, TContext>(TContext context, ILogger logger)
    : IRepositoryBase<TModel, TId>

    where IEntity : class
    where TContext : DbContext
{
    protected readonly TContext _context = context;
    protected readonly ILogger _logger = logger;

    protected DbSet<IEntity> Set => _context.Set<IEntity>();

    protected abstract void ApplyUppdates(TModel model, IEntity entity);

    protected abstract TModel ToModel(IEntity entity);

    protected abstract IEntity ToEntity(TModel model);

    public virtual async Task<TModel> AddAsync(TModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
                throw new NullDomainException("model must be provided.");

            var entity = ToEntity(model);

            await Set.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);

            return ToModel(entity);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            throw;
        }
    }

    public virtual async Task<TModel> UppdteAsync(TId id, TModel model, CancellationToken ct = default)
    {
        try
        {
            if (model is null)
                throw new NullDomainException("model must be provided.");

            var entity = await Set.FindAsync([id], ct)
                ?? throw new NotFoundDomainException($"entity with id '{id}' was not found");

            ApplyUppdates(model, entity);
            await _context.SaveChangesAsync(ct);

            return ToModel(entity);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            throw;
        }
    }

    public virtual async Task<bool> RemoveAsync(TId id, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync([id], ct);
            if (entity is null)
                return false;

            Set.Remove(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            throw;
        }
    }

    public virtual async Task<TModel?> GetByAsync(TId id, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FindAsync([id], ct);

            return entity is null ? default : ToModel(entity);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            throw;
        }
    }

    public virtual async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default)
    {
        try
        {
            var entities = await Set.AsNoTracking().ToListAsync(ct);

            return [.. entities.Select(ToModel)];
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Log(ex);
            throw;
        }
    }
}