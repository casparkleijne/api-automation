namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a service (Dienst) offered by grid operators
/// </summary>
public record Service(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Service name</summary>
    string? Name,
    /// <summary>Service code</summary>
    string? ServiceCode,
    /// <summary>Description of the service</summary>
    string? Description,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Whether the service is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Represents a sub-service (SubDienst) within a service
/// </summary>
public record SubService(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Sub-service name</summary>
    string? Name,
    /// <summary>Description of the sub-service</summary>
    string? Description,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Parent service ID</summary>
    Guid? ServiceId,
    /// <summary>Associated discipline ID</summary>
    Guid? DisciplineId,
    /// <summary>Associated grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Whether EAN code is required</summary>
    bool EanCode,
    /// <summary>Whether EAN check is enabled</summary>
    bool EanCheck,
    /// <summary>Whether notifications are enabled</summary>
    bool Notification,
    /// <summary>Explanation text</summary>
    string? Explanation,
    /// <summary>Whether the sub-service is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a service
/// </summary>
public record CreateServiceRequest(
    string? Name,
    string? ServiceCode,
    string? Description,
    int? Priority,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a service
/// </summary>
public record UpdateServiceRequest(
    string? Name,
    string? ServiceCode,
    string? Description,
    int? Priority,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Request model for creating a sub-service
/// </summary>
public record CreateSubServiceRequest(
    string? Name,
    string? Description,
    int? Priority,
    Guid? ServiceId,
    Guid? DisciplineId,
    Guid? GridOperatorId,
    bool EanCode,
    bool EanCheck,
    bool Notification,
    string? Explanation,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a sub-service
/// </summary>
public record UpdateSubServiceRequest(
    string? Name,
    string? Description,
    int? Priority,
    Guid? ServiceId,
    Guid? DisciplineId,
    Guid? GridOperatorId,
    bool? EanCode,
    bool? EanCheck,
    bool? Notification,
    string? Explanation,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
