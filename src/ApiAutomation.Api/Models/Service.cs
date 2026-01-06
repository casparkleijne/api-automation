namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a service (Dienst) offered by grid operators
/// </summary>
public record Service(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Service code</summary>
    string Code,
    /// <summary>Service name</summary>
    string Name,
    /// <summary>Description of the service</summary>
    string? Description,
    /// <summary>Associated discipline ID</summary>
    Guid? DisciplineId,
    /// <summary>Whether the service is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Represents a sub-service (SubDienst) within a service
/// </summary>
public record SubService(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Sub-service code</summary>
    string Code,
    /// <summary>Sub-service name</summary>
    string Name,
    /// <summary>Description of the sub-service</summary>
    string? Description,
    /// <summary>Parent service ID</summary>
    Guid ServiceId,
    /// <summary>Whether the sub-service is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a service
/// </summary>
public record CreateServiceRequest(
    string Code,
    string Name,
    string? Description,
    Guid? DisciplineId
);

/// <summary>
/// Request model for updating a service
/// </summary>
public record UpdateServiceRequest(
    string? Code,
    string? Name,
    string? Description,
    Guid? DisciplineId,
    bool? IsActive
);

/// <summary>
/// Request model for creating a sub-service
/// </summary>
public record CreateSubServiceRequest(
    string Code,
    string Name,
    string? Description,
    Guid ServiceId
);

/// <summary>
/// Request model for updating a sub-service
/// </summary>
public record UpdateSubServiceRequest(
    string? Code,
    string? Name,
    string? Description,
    bool? IsActive
);
