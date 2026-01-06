namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents branding information for a grid operator
/// </summary>
public record Branding(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Website URL</summary>
    string? WebsiteUri,
    /// <summary>Logo image URL</summary>
    string? LogoImageUri,
    /// <summary>Whether the branding is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating branding
/// </summary>
public record CreateBrandingRequest(
    string? WebsiteUri,
    string? LogoImageUri,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating branding
/// </summary>
public record UpdateBrandingRequest(
    string? WebsiteUri,
    string? LogoImageUri,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
