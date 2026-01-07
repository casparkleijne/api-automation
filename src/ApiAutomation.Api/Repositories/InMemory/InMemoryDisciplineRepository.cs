using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryDisciplineRepository : IDisciplineRepository
{
    private readonly List<Discipline> _items = new()
    {
        new Discipline(
            Id: Guid.Parse("33333333-3333-3333-3333-333333333331"),
            Name: "Electricity",
            Description: "Electrical connections and services",
            Code: "EL",
            IndexCode: 1,
            Priority: 1,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Discipline(
            Id: Guid.Parse("33333333-3333-3333-3333-333333333332"),
            Name: "Gas",
            Description: "Gas connections and services",
            Code: "GAS",
            IndexCode: 2,
            Priority: 2,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Discipline(
            Id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name: "District Heating",
            Description: "District heating connections and services",
            Code: "HEAT",
            IndexCode: 3,
            Priority: 3,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<Discipline>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Discipline?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<Discipline?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Code == code));

    public Task<IEnumerable<Discipline>> GetActiveAsync() =>
        Task.FromResult(_items.Where(x => x.IsActive).AsEnumerable());

    public Task<Discipline> CreateAsync(CreateDisciplineRequest request)
    {
        var item = new Discipline(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            Code: request.Code,
            IndexCode: request.IndexCode,
            Priority: request.Priority,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Discipline?> UpdateAsync(Guid id, UpdateDisciplineRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<Discipline?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            Code = request.Code ?? existing.Code,
            IndexCode = request.IndexCode ?? existing.IndexCode,
            Priority = request.Priority ?? existing.Priority,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<Discipline?>(updated);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult(false);
        _items.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<int> CountAsync() => Task.FromResult(_items.Count);
}
