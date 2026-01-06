namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a product offered by grid operators
/// </summary>
public record Product(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Product code</summary>
    string Code,
    /// <summary>Product name</summary>
    string Name,
    /// <summary>Description of the product</summary>
    string? Description,
    /// <summary>Associated discipline ID</summary>
    Guid? DisciplineId,
    /// <summary>Associated service ID</summary>
    Guid? ServiceId,
    /// <summary>Associated sub-service ID</summary>
    Guid? SubServiceId,
    /// <summary>Associated grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Product type</summary>
    string? ProductType,
    /// <summary>Whether the product is active</summary>
    bool IsActive,
    /// <summary>Effective start date in ISO 8601 format</summary>
    string? EffectiveFrom,
    /// <summary>Effective end date in ISO 8601 format</summary>
    string? EffectiveTo,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a product
/// </summary>
public record CreateProductRequest(
    string Code,
    string Name,
    string? Description,
    Guid? DisciplineId,
    Guid? ServiceId,
    Guid? SubServiceId,
    Guid? GridOperatorId,
    string? ProductType,
    string? EffectiveFrom,
    string? EffectiveTo
);

/// <summary>
/// Request model for updating a product
/// </summary>
public record UpdateProductRequest(
    string? Code,
    string? Name,
    string? Description,
    Guid? DisciplineId,
    Guid? ServiceId,
    Guid? SubServiceId,
    Guid? GridOperatorId,
    string? ProductType,
    bool? IsActive,
    string? EffectiveFrom,
    string? EffectiveTo
);
