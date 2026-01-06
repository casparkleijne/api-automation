namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents general terms and conditions (Algemene Voorwaarden)
/// </summary>
public record GeneralTerms(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Title of the terms</summary>
    string? Title,
    /// <summary>Description/content of the terms</summary>
    string? Description,
    /// <summary>Website URL for full terms</summary>
    string? WebsiteUri,
    /// <summary>Grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Discipline ID</summary>
    Guid? DisciplineId,
    /// <summary>Whether the terms are active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating general terms
/// </summary>
public record CreateGeneralTermsRequest(
    string? Title,
    string? Description,
    string? WebsiteUri,
    Guid? GridOperatorId,
    Guid? DisciplineId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating general terms
/// </summary>
public record UpdateGeneralTermsRequest(
    string? Title,
    string? Description,
    string? WebsiteUri,
    Guid? GridOperatorId,
    Guid? DisciplineId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a disclaimer
/// </summary>
public record Disclaimer(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Disclaimer title</summary>
    string? Title,
    /// <summary>Disclaimer description/content</summary>
    string? Description,
    /// <summary>Website URL for full disclaimer</summary>
    string? WebsiteUri,
    /// <summary>Grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Discipline ID</summary>
    Guid? DisciplineId,
    /// <summary>Whether the disclaimer is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a disclaimer
/// </summary>
public record CreateDisclaimerRequest(
    string? Title,
    string? Description,
    string? WebsiteUri,
    Guid? GridOperatorId,
    Guid? DisciplineId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a disclaimer
/// </summary>
public record UpdateDisclaimerRequest(
    string? Title,
    string? Description,
    string? WebsiteUri,
    Guid? GridOperatorId,
    Guid? DisciplineId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
