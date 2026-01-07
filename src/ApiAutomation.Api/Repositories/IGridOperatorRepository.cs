using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IGridOperatorRepository : ICodeRepository<GridOperator, CreateGridOperatorRequest, UpdateGridOperatorRequest>
{
    Task<IEnumerable<GridOperator>> GetActiveAsync();
}
