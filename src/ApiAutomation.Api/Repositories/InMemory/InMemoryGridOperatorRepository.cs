using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryGridOperatorRepository : IGridOperatorRepository
{
    private readonly List<GridOperator> _items = new()
    {
        new GridOperator(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222221"),
            Name: "Liander",
            GridOperatorCode: "LIA",
            ProcessorCode: "LIA001",
            WebsiteUri: "https://www.liander.nl",
            Department: "Aansluitingen",
            PrimaryEmailAddress: "info@liander.nl",
            ContactPersonId: 1,
            CompanyId: null,
            PrimaryAddressId: 1,
            SecondaryAddressId: null,
            PrimaryPostBoxAddressId: null,
            SecondaryPostBoxAddressId: null,
            CustomerServiceId: null,
            CustomerService: null,
            AansluitGereedDatumActive: true,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new GridOperator(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name: "Stedin",
            GridOperatorCode: "STD",
            ProcessorCode: "STD001",
            WebsiteUri: "https://www.stedin.net",
            Department: "Aansluitingen",
            PrimaryEmailAddress: "info@stedin.net",
            ContactPersonId: 2,
            CompanyId: null,
            PrimaryAddressId: 2,
            SecondaryAddressId: null,
            PrimaryPostBoxAddressId: null,
            SecondaryPostBoxAddressId: null,
            CustomerServiceId: null,
            CustomerService: null,
            AansluitGereedDatumActive: true,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<GridOperator>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<GridOperator?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<GridOperator?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.GridOperatorCode == code));

    public Task<IEnumerable<GridOperator>> GetActiveAsync() =>
        Task.FromResult(_items.Where(x => x.IsActive).AsEnumerable());

    public Task<GridOperator> CreateAsync(CreateGridOperatorRequest request)
    {
        var item = new GridOperator(
            Id: Guid.NewGuid(),
            Name: request.Name,
            GridOperatorCode: request.GridOperatorCode,
            ProcessorCode: request.ProcessorCode,
            WebsiteUri: request.WebsiteUri,
            Department: request.Department,
            PrimaryEmailAddress: request.PrimaryEmailAddress,
            ContactPersonId: request.ContactPersonId,
            CompanyId: request.CompanyId,
            PrimaryAddressId: request.PrimaryAddressId,
            SecondaryAddressId: request.SecondaryAddressId,
            PrimaryPostBoxAddressId: request.PrimaryPostBoxAddressId,
            SecondaryPostBoxAddressId: request.SecondaryPostBoxAddressId,
            CustomerServiceId: request.CustomerServiceId,
            CustomerService: null,
            AansluitGereedDatumActive: request.AansluitGereedDatumActive,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<GridOperator?> UpdateAsync(Guid id, UpdateGridOperatorRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<GridOperator?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            GridOperatorCode = request.GridOperatorCode ?? existing.GridOperatorCode,
            ProcessorCode = request.ProcessorCode ?? existing.ProcessorCode,
            WebsiteUri = request.WebsiteUri ?? existing.WebsiteUri,
            Department = request.Department ?? existing.Department,
            PrimaryEmailAddress = request.PrimaryEmailAddress ?? existing.PrimaryEmailAddress,
            ContactPersonId = request.ContactPersonId ?? existing.ContactPersonId,
            CompanyId = request.CompanyId ?? existing.CompanyId,
            PrimaryAddressId = request.PrimaryAddressId ?? existing.PrimaryAddressId,
            SecondaryAddressId = request.SecondaryAddressId ?? existing.SecondaryAddressId,
            PrimaryPostBoxAddressId = request.PrimaryPostBoxAddressId ?? existing.PrimaryPostBoxAddressId,
            SecondaryPostBoxAddressId = request.SecondaryPostBoxAddressId ?? existing.SecondaryPostBoxAddressId,
            CustomerServiceId = request.CustomerServiceId ?? existing.CustomerServiceId,
            AansluitGereedDatumActive = request.AansluitGereedDatumActive ?? existing.AansluitGereedDatumActive,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<GridOperator?>(updated);
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
