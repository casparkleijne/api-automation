namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a physical address
/// </summary>
public record Address(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Street name</summary>
    string? Street,
    /// <summary>House number</summary>
    string Number,
    /// <summary>House number suffix/addition</summary>
    string? Suffix,
    /// <summary>Postal code (Dutch format: 1234AB)</summary>
    string PostalCode,
    /// <summary>City name</summary>
    string? City,
    /// <summary>Country code</summary>
    string? Country,
    /// <summary>Whether the address is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating an address
/// </summary>
public record CreateAddressRequest(
    string? Street,
    string Number,
    string? Suffix,
    string PostalCode,
    string? City,
    string? Country,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating an address
/// </summary>
public record UpdateAddressRequest(
    string? Street,
    string? Number,
    string? Suffix,
    string? PostalCode,
    string? City,
    string? Country,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a post box address
/// </summary>
public record PostBoxAddress(
    Guid Id,
    string? PostBox,
    string? PostalCode,
    string? City,
    string? Country,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents geographic coordinates
/// </summary>
public record Coordinates(
    double Latitude,
    double Longitude
);
