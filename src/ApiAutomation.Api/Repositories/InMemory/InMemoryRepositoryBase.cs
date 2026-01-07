namespace ApiAutomation.Api.Repositories.InMemory;

/// <summary>
/// Generic in-memory repository base class implementing common CRUD operations
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <typeparam name="TCreate">Create request type</typeparam>
/// <typeparam name="TUpdate">Update request type</typeparam>
public abstract class InMemoryRepositoryBase<TEntity, TCreate, TUpdate>
    : IRepository<TEntity, TCreate, TUpdate>
    where TEntity : class, IEntity
{
    protected readonly List<TEntity> Items = new();
    private readonly object _lock = new();

    public Task<IEnumerable<TEntity>> GetAllAsync() =>
        Task.FromResult(Items.AsEnumerable());

    public Task<TEntity?> GetByIdAsync(Guid id) =>
        Task.FromResult(Items.FirstOrDefault(x => x.Id == id));

    public Task<TEntity> CreateAsync(TCreate request)
    {
        var entity = MapToEntity(request);
        lock (_lock) { Items.Add(entity); }
        return Task.FromResult(entity);
    }

    public Task<TEntity?> UpdateAsync(Guid id, TUpdate request)
    {
        lock (_lock)
        {
            var index = Items.FindIndex(x => x.Id == id);
            if (index < 0) return Task.FromResult<TEntity?>(null);

            var updated = ApplyUpdate(Items[index], request);
            Items[index] = updated;
            return Task.FromResult<TEntity?>(updated);
        }
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        lock (_lock)
        {
            var index = Items.FindIndex(x => x.Id == id);
            if (index < 0) return Task.FromResult(false);
            Items.RemoveAt(index);
            return Task.FromResult(true);
        }
    }

    public Task<int> CountAsync() => Task.FromResult(Items.Count);

    /// <summary>
    /// Maps a create request to a new entity. Override in derived class.
    /// </summary>
    protected abstract TEntity MapToEntity(TCreate request);

    /// <summary>
    /// Applies update request to existing entity. Override in derived class.
    /// </summary>
    protected abstract TEntity ApplyUpdate(TEntity existing, TUpdate request);

    /// <summary>
    /// Seeds initial data. Call in constructor of derived class.
    /// </summary>
    protected void Seed(params TEntity[] entities)
    {
        Items.AddRange(entities);
    }
}

/// <summary>
/// Generic in-memory repository with code-based lookup
/// </summary>
public abstract class InMemoryCodeRepositoryBase<TEntity, TCreate, TUpdate>
    : InMemoryRepositoryBase<TEntity, TCreate, TUpdate>, ICodeRepository<TEntity, TCreate, TUpdate>
    where TEntity : class, IEntity, ICodeEntity
{
    public Task<TEntity?> GetByCodeAsync(string code) =>
        Task.FromResult(Items.FirstOrDefault(x =>
            x.Code?.Equals(code, StringComparison.OrdinalIgnoreCase) ?? false));
}
