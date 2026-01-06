namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a price component (PrijsComponent) for products and services
/// </summary>
public record PriceComponent(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Price component code</summary>
    string Code,
    /// <summary>Price component name</summary>
    string Name,
    /// <summary>Description of the price component</summary>
    string? Description,
    /// <summary>Associated product ID</summary>
    Guid? ProductId,
    /// <summary>Associated service ID</summary>
    Guid? ServiceId,
    /// <summary>Associated grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Price amount</summary>
    decimal Amount,
    /// <summary>Currency code (ISO 4217)</summary>
    string Currency,
    /// <summary>Unit of measurement</summary>
    string? Unit,
    /// <summary>VAT percentage</summary>
    decimal? VatPercentage,
    /// <summary>Whether VAT is included in the amount</summary>
    bool VatIncluded,
    /// <summary>Price type (fixed, variable, per-unit)</summary>
    string? PriceType,
    /// <summary>Effective start date in ISO 8601 format</summary>
    string? EffectiveFrom,
    /// <summary>Effective end date in ISO 8601 format</summary>
    string? EffectiveTo,
    /// <summary>Whether the price component is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a price component
/// </summary>
public record CreatePriceComponentRequest(
    string Code,
    string Name,
    string? Description,
    Guid? ProductId,
    Guid? ServiceId,
    Guid? GridOperatorId,
    decimal Amount,
    string Currency,
    string? Unit,
    decimal? VatPercentage,
    bool VatIncluded,
    string? PriceType,
    string? EffectiveFrom,
    string? EffectiveTo
);

/// <summary>
/// Request model for updating a price component
/// </summary>
public record UpdatePriceComponentRequest(
    string? Code,
    string? Name,
    string? Description,
    Guid? ProductId,
    Guid? ServiceId,
    Guid? GridOperatorId,
    decimal? Amount,
    string? Currency,
    string? Unit,
    decimal? VatPercentage,
    bool? VatIncluded,
    string? PriceType,
    string? EffectiveFrom,
    string? EffectiveTo,
    bool? IsActive
);
