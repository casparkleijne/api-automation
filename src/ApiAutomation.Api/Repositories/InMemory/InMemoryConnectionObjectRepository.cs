using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryConnectionObjectRepository : IConnectionObjectRepository
{
    private readonly List<ConnectionObject> _items = new()
    {
        new ConnectionObject(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name: "Residential",
            Description: "Residential connection object",
            Code: "RES",
            Priority: 1,
            ProfileType: 1,
            AddressType: 1,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new ConnectionObject(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111112"),
            Name: "Commercial",
            Description: "Commercial connection object",
            Code: "COM",
            Priority: 2,
            ProfileType: 2,
            AddressType: 1,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<ConnectionObject>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<ConnectionObject?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<ConnectionObject?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Code == code));

    public Task<IEnumerable<ConnectionObject>> GetByGridOperatorIdAsync(Guid gridOperatorId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<ConnectionObject> CreateAsync(CreateConnectionObjectRequest request)
    {
        var item = new ConnectionObject(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            Code: request.Code,
            Priority: request.Priority,
            ProfileType: request.ProfileType,
            AddressType: request.AddressType,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<ConnectionObject?> UpdateAsync(Guid id, UpdateConnectionObjectRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<ConnectionObject?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            Code = request.Code ?? existing.Code,
            Priority = request.Priority ?? existing.Priority,
            ProfileType = request.ProfileType ?? existing.ProfileType,
            AddressType = request.AddressType ?? existing.AddressType,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<ConnectionObject?>(updated);
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
