namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a process variant for handling different workflows
/// </summary>
public record ProcessVariant(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Process variant name</summary>
    string? Name,
    /// <summary>Description of the process variant</summary>
    string? Description,
    /// <summary>Whether the process variant is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a process variant
/// </summary>
public record CreateProcessVariantRequest(
    string? Name,
    string? Description,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a process variant
/// </summary>
public record UpdateProcessVariantRequest(
    string? Name,
    string? Description,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
