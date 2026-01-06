namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents availability/capacity information for a grid operator
/// </summary>
public record Availability(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Start date of availability period</summary>
    string? StartDate,
    /// <summary>End date of availability period</summary>
    string? EndDate,
    /// <summary>Grid operator ID</summary>
    Guid? GridOperatorId
);

/// <summary>
/// Request model for creating availability
/// </summary>
public record CreateAvailabilityRequest(
    string? StartDate,
    string? EndDate,
    Guid? GridOperatorId
);

/// <summary>
/// Request model for updating availability
/// </summary>
public record UpdateAvailabilityRequest(
    string? StartDate,
    string? EndDate,
    Guid? GridOperatorId
);

/// <summary>
/// Represents available grid operator disciplines for a location
/// </summary>
public record AvailableGridOperatorDiscipline(
    Guid Id,
    Guid? GridOperatorId,
    Guid? DisciplineId,
    Guid? ConnectionObjectId,
    bool IsAvailable,
    string? Message
);
