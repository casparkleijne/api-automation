using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _items = new()
    {
        new Product(
            Id: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            Name: "Standard Electricity Connection",
            Description: "Standard electricity connection for residential use",
            WebsiteLink: "https://example.com/products/standard-el",
            WebsiteText: "More info",
            LongTitle: "Standard Electricity Connection - Residential",
            ShortTitle: "Std Electricity",
            ProductCode: "EL-STD-001",
            TariffCode: "T001",
            CompanyIdentifier: "COMP001",
            IsLargeConsumer: false,
            IsStandard: true,
            Priority: 1,
            IsPriceIndicative: false,
            SubServiceId: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Product(
            Id: Guid.Parse("66666666-6666-6666-6666-666666666662"),
            Name: "Heavy Duty Electricity Connection",
            Description: "Heavy duty electricity connection for commercial use",
            WebsiteLink: "https://example.com/products/heavy-el",
            WebsiteText: "More info",
            LongTitle: "Heavy Duty Electricity Connection - Commercial",
            ShortTitle: "Heavy Electricity",
            ProductCode: "EL-HD-001",
            TariffCode: "T002",
            CompanyIdentifier: "COMP001",
            IsLargeConsumer: true,
            IsStandard: false,
            Priority: 2,
            IsPriceIndicative: true,
            SubServiceId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<Product>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Product?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<Product?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.ProductCode == code));

    public Task<IEnumerable<Product>> GetByDisciplineIdAsync(Guid disciplineId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<IEnumerable<Product>> GetByServiceIdAsync(Guid serviceId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<IEnumerable<Product>> GetByGridOperatorIdAsync(Guid gridOperatorId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<IEnumerable<Product>> GetActiveAsync() =>
        Task.FromResult(_items.Where(x => x.IsActive).AsEnumerable());

    public Task<Product> CreateAsync(CreateProductRequest request)
    {
        var item = new Product(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            WebsiteLink: request.WebsiteLink,
            WebsiteText: request.WebsiteText,
            LongTitle: request.LongTitle,
            ShortTitle: request.ShortTitle,
            ProductCode: request.ProductCode,
            TariffCode: request.TariffCode,
            CompanyIdentifier: request.CompanyIdentifier,
            IsLargeConsumer: request.IsLargeConsumer,
            IsStandard: request.IsStandard,
            Priority: request.Priority,
            IsPriceIndicative: request.IsPriceIndicative,
            SubServiceId: request.SubServiceId,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Product?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<Product?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            WebsiteLink = request.WebsiteLink ?? existing.WebsiteLink,
            WebsiteText = request.WebsiteText ?? existing.WebsiteText,
            LongTitle = request.LongTitle ?? existing.LongTitle,
            ShortTitle = request.ShortTitle ?? existing.ShortTitle,
            ProductCode = request.ProductCode ?? existing.ProductCode,
            TariffCode = request.TariffCode ?? existing.TariffCode,
            CompanyIdentifier = request.CompanyIdentifier ?? existing.CompanyIdentifier,
            IsLargeConsumer = request.IsLargeConsumer ?? existing.IsLargeConsumer,
            IsStandard = request.IsStandard ?? existing.IsStandard,
            Priority = request.Priority ?? existing.Priority,
            IsPriceIndicative = request.IsPriceIndicative ?? existing.IsPriceIndicative,
            SubServiceId = request.SubServiceId ?? existing.SubServiceId,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<Product?>(updated);
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
