namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a product offered by grid operators
/// </summary>
public record Product(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Product name</summary>
    string? Name,
    /// <summary>Description of the product</summary>
    string? Description,
    /// <summary>Website link for more information</summary>
    string? WebsiteLink,
    /// <summary>Website link text</summary>
    string? WebsiteText,
    /// <summary>Long title for display</summary>
    string? LongTitle,
    /// <summary>Short title for display</summary>
    string? ShortTitle,
    /// <summary>Product code</summary>
    string? ProductCode,
    /// <summary>Tariff code</summary>
    string? TariffCode,
    /// <summary>Company-specific identifier</summary>
    string? CompanyIdentifier,
    /// <summary>Whether this is for large consumers</summary>
    bool IsLargeConsumer,
    /// <summary>Whether this is the standard product</summary>
    bool IsStandard,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Whether price is indicative</summary>
    bool IsPriceIndicative,
    /// <summary>Associated sub-service ID</summary>
    Guid? SubServiceId,
    /// <summary>Whether the product is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a product
/// </summary>
public record CreateProductRequest(
    string? Name,
    string? Description,
    string? WebsiteLink,
    string? WebsiteText,
    string? LongTitle,
    string? ShortTitle,
    string? ProductCode,
    string? TariffCode,
    string? CompanyIdentifier,
    bool IsLargeConsumer,
    bool IsStandard,
    int? Priority,
    bool IsPriceIndicative,
    Guid? SubServiceId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a product
/// </summary>
public record UpdateProductRequest(
    string? Name,
    string? Description,
    string? WebsiteLink,
    string? WebsiteText,
    string? LongTitle,
    string? ShortTitle,
    string? ProductCode,
    string? TariffCode,
    string? CompanyIdentifier,
    bool? IsLargeConsumer,
    bool? IsStandard,
    int? Priority,
    bool? IsPriceIndicative,
    Guid? SubServiceId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a link between products and process variants
/// </summary>
public record ProductProcessVariant(
    Guid Id,
    Guid ProductId,
    Guid ProcessVariantId,
    bool IsActive,
    string StartDate,
    string EndDate
);
