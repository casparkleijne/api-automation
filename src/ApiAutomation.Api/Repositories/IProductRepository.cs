using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IProductRepository : ICodeRepository<Product, CreateProductRequest, UpdateProductRequest>
{
    Task<IEnumerable<Product>> GetByDisciplineIdAsync(Guid disciplineId);
    Task<IEnumerable<Product>> GetByServiceIdAsync(Guid serviceId);
    Task<IEnumerable<Product>> GetByGridOperatorIdAsync(Guid gridOperatorId);
    Task<IEnumerable<Product>> GetActiveAsync();
}
