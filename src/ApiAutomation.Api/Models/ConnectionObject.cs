namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a connection object (Aansluitingsobject) in the energy grid
/// </summary>
public record ConnectionObject(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Name of the connection object</summary>
    string? Name,
    /// <summary>Description of the connection object</summary>
    string? Description,
    /// <summary>Connection object code</summary>
    string? Code,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Profile type (e.g., residential, commercial)</summary>
    int? ProfileType,
    /// <summary>Address type</summary>
    int? AddressType,
    /// <summary>Whether the connection object is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a connection object
/// </summary>
public record CreateConnectionObjectRequest(
    string? Name,
    string? Description,
    string? Code,
    int? Priority,
    int? ProfileType,
    int? AddressType,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a connection object
/// </summary>
public record UpdateConnectionObjectRequest(
    string? Name,
    string? Description,
    string? Code,
    int? Priority,
    int? ProfileType,
    int? AddressType,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a link between connection objects and products
/// </summary>
public record ConnectionObjectProduct(
    Guid Id,
    Guid ConnectionObjectId,
    Guid ProductId,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents user type settings for a connection object
/// </summary>
public record ConnectionObjectUserType(
    Guid Id,
    Guid ConnectionObjectId,
    string? UserType,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents grid operator settings for a connection object
/// </summary>
public record ConnectionObjectGridOperatorSetting(
    Guid Id,
    Guid ConnectionObjectId,
    Guid? GridOperatorId,
    string? SettingKey,
    string? SettingValue,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a process variant for a connection object
/// </summary>
public record ConnectionObjectProcessVariant(
    Guid Id,
    Guid ConnectionObjectId,
    Guid? ProcessVariantId,
    string? Name,
    string? Description,
    int? Priority,
    bool IsActive,
    string StartDate,
    string EndDate
);
