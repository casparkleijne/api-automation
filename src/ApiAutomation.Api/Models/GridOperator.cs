namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a grid operator (Netbeheerder) responsible for managing energy infrastructure
/// </summary>
public record GridOperator(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Grid operator code</summary>
    string Code,
    /// <summary>Grid operator name</summary>
    string Name,
    /// <summary>Description of the grid operator</summary>
    string? Description,
    /// <summary>EAN code prefix for this operator</summary>
    string? EanPrefix,
    /// <summary>Contact email</summary>
    string? Email,
    /// <summary>Contact phone number</summary>
    string? Phone,
    /// <summary>Website URL</summary>
    string? Website,
    /// <summary>Whether the grid operator is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a grid operator
/// </summary>
public record CreateGridOperatorRequest(
    string Code,
    string Name,
    string? Description,
    string? EanPrefix,
    string? Email,
    string? Phone,
    string? Website
);

/// <summary>
/// Request model for updating a grid operator
/// </summary>
public record UpdateGridOperatorRequest(
    string? Code,
    string? Name,
    string? Description,
    string? EanPrefix,
    string? Email,
    string? Phone,
    string? Website,
    bool? IsActive
);
