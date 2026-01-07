namespace ApiAutomation.Api.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TCreate">Create request type</typeparam>
/// <typeparam name="TUpdate">Update request type</typeparam>
public interface IRepository<T, TCreate, TUpdate>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> CreateAsync(TCreate request);
    Task<T?> UpdateAsync(Guid id, TUpdate request);
    Task<bool> DeleteAsync(Guid id);
    Task<int> CountAsync();
}

/// <summary>
/// Repository interface with code-based lookup
/// </summary>
public interface ICodeRepository<T, TCreate, TUpdate> : IRepository<T, TCreate, TUpdate>
{
    Task<T?> GetByCodeAsync(string code);
}
