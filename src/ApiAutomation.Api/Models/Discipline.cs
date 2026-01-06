namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a discipline (e.g., Electricity, Gas) in the energy sector
/// </summary>
public record Discipline(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Discipline name</summary>
    string? Name,
    /// <summary>Description of the discipline</summary>
    string? Description,
    /// <summary>Discipline code (e.g., EL, GAS)</summary>
    string Code,
    /// <summary>Index code for ordering</summary>
    int? IndexCode,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Whether the discipline is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a discipline
/// </summary>
public record CreateDisciplineRequest(
    string? Name,
    string? Description,
    string Code,
    int? IndexCode,
    int? Priority,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a discipline
/// </summary>
public record UpdateDisciplineRequest(
    string? Name,
    string? Description,
    string? Code,
    int? IndexCode,
    int? Priority,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents link between disciplines and connection objects
/// </summary>
public record DisciplineConnectionObject(
    Guid Id,
    Guid DisciplineId,
    Guid ConnectionObjectId,
    bool IsActive,
    string StartDate,
    string EndDate
);
