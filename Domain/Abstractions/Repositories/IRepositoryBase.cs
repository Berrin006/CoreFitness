namespace Domain.Abstractions.Repositories;

public interface IRepositoryBase<TModel, TId>
{
    Task<TModel> AddAsync(TModel model, CancellationToken ct = default);

    Task<TModel> UppdteAsync(TId id, TModel model, CancellationToken ct = default);

    Task<bool> RemoveAsync(TId id, CancellationToken ct = default);

    Task<TModel?> GetByAsync(TId id, CancellationToken ct = default);

    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default);

}
