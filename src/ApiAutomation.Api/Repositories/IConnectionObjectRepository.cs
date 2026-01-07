using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IConnectionObjectRepository : ICodeRepository<ConnectionObject, CreateConnectionObjectRequest, UpdateConnectionObjectRequest>
{
    Task<IEnumerable<ConnectionObject>> GetByGridOperatorIdAsync(Guid gridOperatorId);
}
