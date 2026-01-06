namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a discipline (e.g., Electricity, Gas) in the energy sector
/// </summary>
public record Discipline(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Discipline code (e.g., EL, GAS)</summary>
    string Code,
    /// <summary>Discipline name</summary>
    string Name,
    /// <summary>Description of the discipline</summary>
    string? Description,
    /// <summary>Whether the discipline is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a discipline
/// </summary>
public record CreateDisciplineRequest(
    string Code,
    string Name,
    string? Description
);

/// <summary>
/// Request model for updating a discipline
/// </summary>
public record UpdateDisciplineRequest(
    string? Code,
    string? Name,
    string? Description,
    bool? IsActive
);
