using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryPriceComponentRepository : IPriceComponentRepository
{
    private readonly List<PriceComponent> _items = new()
    {
        new PriceComponent(
            Id: Guid.Parse("99999999-9999-9999-9999-999999999991"),
            Name: "Connection Fee",
            Description: "One-time connection fee",
            IsEnabled: true,
            PriceComponentCode: "PC001",
            CompanyIdentifier: "COMP001",
            TariffCode: "T001",
            MinValue: null,
            MaxValue: null,
            DefaultValue: null,
            BaseValue: null,
            BasePrice: 500.00,
            BasePriceMonthly: null,
            UnitPrice: null,
            UnitPriceMonthly: null,
            VatCodeId: null,
            QuestionInfoId: null,
            QuestionInfoPositionId: null,
            IsUniqueForInvoice: true,
            IsFalloutPriceComponent: false,
            ProcessVariantId: null,
            ProductId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            QuestionId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new PriceComponent(
            Id: Guid.Parse("99999999-9999-9999-9999-999999999992"),
            Name: "Monthly Network Fee",
            Description: "Monthly network usage fee",
            IsEnabled: true,
            PriceComponentCode: "PC002",
            CompanyIdentifier: "COMP001",
            TariffCode: "T002",
            MinValue: null,
            MaxValue: null,
            DefaultValue: null,
            BaseValue: null,
            BasePrice: null,
            BasePriceMonthly: 25.00,
            UnitPrice: null,
            UnitPriceMonthly: null,
            VatCodeId: null,
            QuestionInfoId: null,
            QuestionInfoPositionId: null,
            IsUniqueForInvoice: false,
            IsFalloutPriceComponent: false,
            ProcessVariantId: null,
            ProductId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            QuestionId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<PriceComponent>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<PriceComponent?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<PriceComponent?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.PriceComponentCode == code));

    public Task<IEnumerable<PriceComponent>> GetByProductIdAsync(Guid productId) =>
        Task.FromResult(_items.Where(x => x.ProductId == productId).AsEnumerable());

    public Task<IEnumerable<PriceComponent>> GetByGridOperatorIdAsync(Guid gridOperatorId) =>
        Task.FromResult(_items.AsEnumerable()); // Simplified for demo

    public Task<IEnumerable<PriceComponent>> GetActiveAsync() =>
        Task.FromResult(_items.Where(x => x.IsActive).AsEnumerable());

    public Task<PriceComponent> CreateAsync(CreatePriceComponentRequest request)
    {
        var item = new PriceComponent(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            IsEnabled: request.IsEnabled,
            PriceComponentCode: request.PriceComponentCode,
            CompanyIdentifier: request.CompanyIdentifier,
            TariffCode: request.TariffCode,
            MinValue: request.MinValue,
            MaxValue: request.MaxValue,
            DefaultValue: request.DefaultValue,
            BaseValue: request.BaseValue,
            BasePrice: request.BasePrice,
            BasePriceMonthly: request.BasePriceMonthly,
            UnitPrice: request.UnitPrice,
            UnitPriceMonthly: request.UnitPriceMonthly,
            VatCodeId: request.VatCodeId,
            QuestionInfoId: request.QuestionInfoId,
            QuestionInfoPositionId: request.QuestionInfoPositionId,
            IsUniqueForInvoice: request.IsUniqueForInvoice,
            IsFalloutPriceComponent: request.IsFalloutPriceComponent,
            ProcessVariantId: request.ProcessVariantId,
            ProductId: request.ProductId,
            QuestionId: request.QuestionId,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<PriceComponent?> UpdateAsync(Guid id, UpdatePriceComponentRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<PriceComponent?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            IsEnabled = request.IsEnabled ?? existing.IsEnabled,
            PriceComponentCode = request.PriceComponentCode ?? existing.PriceComponentCode,
            CompanyIdentifier = request.CompanyIdentifier ?? existing.CompanyIdentifier,
            TariffCode = request.TariffCode ?? existing.TariffCode,
            MinValue = request.MinValue ?? existing.MinValue,
            MaxValue = request.MaxValue ?? existing.MaxValue,
            DefaultValue = request.DefaultValue ?? existing.DefaultValue,
            BaseValue = request.BaseValue ?? existing.BaseValue,
            BasePrice = request.BasePrice ?? existing.BasePrice,
            BasePriceMonthly = request.BasePriceMonthly ?? existing.BasePriceMonthly,
            UnitPrice = request.UnitPrice ?? existing.UnitPrice,
            UnitPriceMonthly = request.UnitPriceMonthly ?? existing.UnitPriceMonthly,
            VatCodeId = request.VatCodeId ?? existing.VatCodeId,
            QuestionInfoId = request.QuestionInfoId ?? existing.QuestionInfoId,
            QuestionInfoPositionId = request.QuestionInfoPositionId ?? existing.QuestionInfoPositionId,
            IsUniqueForInvoice = request.IsUniqueForInvoice ?? existing.IsUniqueForInvoice,
            IsFalloutPriceComponent = request.IsFalloutPriceComponent ?? existing.IsFalloutPriceComponent,
            ProcessVariantId = request.ProcessVariantId ?? existing.ProcessVariantId,
            ProductId = request.ProductId ?? existing.ProductId,
            QuestionId = request.QuestionId ?? existing.QuestionId,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<PriceComponent?>(updated);
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
