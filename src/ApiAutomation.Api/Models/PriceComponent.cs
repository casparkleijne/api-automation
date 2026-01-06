namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a price component (PrijsComponent) for products and services
/// </summary>
public record PriceComponent(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Price component name</summary>
    string? Name,
    /// <summary>Description of the price component</summary>
    string? Description,
    /// <summary>Whether the price component is enabled</summary>
    bool IsEnabled,
    /// <summary>Price component code</summary>
    string? PriceComponentCode,
    /// <summary>Company-specific identifier</summary>
    string? CompanyIdentifier,
    /// <summary>Tariff code</summary>
    string? TariffCode,
    /// <summary>Minimum value</summary>
    double? MinValue,
    /// <summary>Maximum value</summary>
    double? MaxValue,
    /// <summary>Default value</summary>
    double? DefaultValue,
    /// <summary>Base value</summary>
    double? BaseValue,
    /// <summary>Base price (one-time)</summary>
    double? BasePrice,
    /// <summary>Base price (monthly)</summary>
    double? BasePriceMonthly,
    /// <summary>Unit price (one-time)</summary>
    double? UnitPrice,
    /// <summary>Unit price (monthly)</summary>
    double? UnitPriceMonthly,
    /// <summary>VAT code ID</summary>
    Guid? VatCodeId,
    /// <summary>Question info ID</summary>
    Guid? QuestionInfoId,
    /// <summary>Question info position ID</summary>
    int? QuestionInfoPositionId,
    /// <summary>Whether unique for invoice</summary>
    bool IsUniqueForInvoice,
    /// <summary>Whether this is a fallout price component</summary>
    bool IsFalloutPriceComponent,
    /// <summary>Process variant ID</summary>
    Guid? ProcessVariantId,
    /// <summary>Product ID</summary>
    Guid? ProductId,
    /// <summary>Question ID</summary>
    Guid? QuestionId,
    /// <summary>Whether the price component is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a price component
/// </summary>
public record CreatePriceComponentRequest(
    string? Name,
    string? Description,
    bool IsEnabled,
    string? PriceComponentCode,
    string? CompanyIdentifier,
    string? TariffCode,
    double? MinValue,
    double? MaxValue,
    double? DefaultValue,
    double? BaseValue,
    double? BasePrice,
    double? BasePriceMonthly,
    double? UnitPrice,
    double? UnitPriceMonthly,
    Guid? VatCodeId,
    Guid? QuestionInfoId,
    int? QuestionInfoPositionId,
    bool IsUniqueForInvoice,
    bool IsFalloutPriceComponent,
    Guid? ProcessVariantId,
    Guid? ProductId,
    Guid? QuestionId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a price component
/// </summary>
public record UpdatePriceComponentRequest(
    string? Name,
    string? Description,
    bool? IsEnabled,
    string? PriceComponentCode,
    string? CompanyIdentifier,
    string? TariffCode,
    double? MinValue,
    double? MaxValue,
    double? DefaultValue,
    double? BaseValue,
    double? BasePrice,
    double? BasePriceMonthly,
    double? UnitPrice,
    double? UnitPriceMonthly,
    Guid? VatCodeId,
    Guid? QuestionInfoId,
    int? QuestionInfoPositionId,
    bool? IsUniqueForInvoice,
    bool? IsFalloutPriceComponent,
    Guid? ProcessVariantId,
    Guid? ProductId,
    Guid? QuestionId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents an action for a price component
/// </summary>
public record PriceComponentAction(
    Guid Id,
    Guid? PriceComponentId,
    Guid? ActionConditionId,
    string? ActionType,
    string? ActionValue,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a VAT code (BTW code)
/// </summary>
public record VatCode(
    Guid Id,
    string? Name,
    string? Description,
    double Value,
    bool IsEnabled,
    int? Priority,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for creating a VAT code
/// </summary>
public record CreateVatCodeRequest(
    string? Name,
    string? Description,
    double Value,
    bool IsEnabled,
    int? Priority,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a VAT code
/// </summary>
public record UpdateVatCodeRequest(
    string? Name,
    string? Description,
    double? Value,
    bool? IsEnabled,
    int? Priority,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
