using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IServiceRepository : ICodeRepository<Service, CreateServiceRequest, UpdateServiceRequest>
{
    Task<IEnumerable<Service>> GetByDisciplineIdAsync(Guid disciplineId);
}

public interface ISubServiceRepository : IRepository<SubService, CreateSubServiceRequest, UpdateSubServiceRequest>
{
    Task<IEnumerable<SubService>> GetByServiceIdAsync(Guid serviceId);
}
