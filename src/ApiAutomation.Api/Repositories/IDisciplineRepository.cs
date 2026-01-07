using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IDisciplineRepository : ICodeRepository<Discipline, CreateDisciplineRequest, UpdateDisciplineRequest>
{
    Task<IEnumerable<Discipline>> GetActiveAsync();
}
