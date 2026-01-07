using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryServiceRepository : IServiceRepository
{
    private readonly List<Service> _items = new()
    {
        new Service(
            Id: Guid.Parse("44444444-4444-4444-4444-444444444441"),
            Name: "New Connection",
            ServiceCode: "NEW",
            Description: "Request a new energy connection",
            Priority: 1,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Service(
            Id: Guid.Parse("44444444-4444-4444-4444-444444444442"),
            Name: "Modify Connection",
            ServiceCode: "MOD",
            Description: "Modify an existing energy connection",
            Priority: 2,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Service(
            Id: Guid.Parse("44444444-4444-4444-4444-444444444443"),
            Name: "Remove Connection",
            ServiceCode: "REM",
            Description: "Remove an existing energy connection",
            Priority: 3,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<Service>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Service?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<Service?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.ServiceCode == code));

    public Task<IEnumerable<Service>> GetByDisciplineIdAsync(Guid disciplineId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<Service> CreateAsync(CreateServiceRequest request)
    {
        var item = new Service(
            Id: Guid.NewGuid(),
            Name: request.Name,
            ServiceCode: request.ServiceCode,
            Description: request.Description,
            Priority: request.Priority,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Service?> UpdateAsync(Guid id, UpdateServiceRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<Service?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            ServiceCode = request.ServiceCode ?? existing.ServiceCode,
            Description = request.Description ?? existing.Description,
            Priority = request.Priority ?? existing.Priority,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<Service?>(updated);
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

public class InMemorySubServiceRepository : ISubServiceRepository
{
    private readonly List<SubService> _items = new()
    {
        new SubService(
            Id: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            Name: "Standard Connection",
            Description: "Standard new connection request",
            Priority: 1,
            ServiceId: Guid.Parse("44444444-4444-4444-4444-444444444441"),
            DisciplineId: Guid.Parse("33333333-3333-3333-3333-333333333331"),
            GridOperatorId: null,
            EanCode: true,
            EanCheck: true,
            Notification: true,
            Explanation: "Standard electricity connection",
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<SubService>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<SubService?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<IEnumerable<SubService>> GetByServiceIdAsync(Guid serviceId) =>
        Task.FromResult(_items.Where(x => x.ServiceId == serviceId).AsEnumerable());

    public Task<SubService> CreateAsync(CreateSubServiceRequest request)
    {
        var item = new SubService(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            Priority: request.Priority,
            ServiceId: request.ServiceId,
            DisciplineId: request.DisciplineId,
            GridOperatorId: request.GridOperatorId,
            EanCode: request.EanCode,
            EanCheck: request.EanCheck,
            Notification: request.Notification,
            Explanation: request.Explanation,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<SubService?> UpdateAsync(Guid id, UpdateSubServiceRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<SubService?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            Priority = request.Priority ?? existing.Priority,
            ServiceId = request.ServiceId ?? existing.ServiceId,
            DisciplineId = request.DisciplineId ?? existing.DisciplineId,
            GridOperatorId = request.GridOperatorId ?? existing.GridOperatorId,
            EanCode = request.EanCode ?? existing.EanCode,
            EanCheck = request.EanCheck ?? existing.EanCheck,
            Notification = request.Notification ?? existing.Notification,
            Explanation = request.Explanation ?? existing.Explanation,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<SubService?>(updated);
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
