using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IPriceComponentRepository : ICodeRepository<PriceComponent, CreatePriceComponentRequest, UpdatePriceComponentRequest>
{
    Task<IEnumerable<PriceComponent>> GetByProductIdAsync(Guid productId);
    Task<IEnumerable<PriceComponent>> GetByGridOperatorIdAsync(Guid gridOperatorId);
    Task<IEnumerable<PriceComponent>> GetActiveAsync();
}
