namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a connection object (Aansluitingsobject) in the energy grid
/// </summary>
public record ConnectionObject(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>EAN code for the connection</summary>
    string? Ean,
    /// <summary>Postal code</summary>
    string? PostalCode,
    /// <summary>House number</summary>
    int? HouseNumber,
    /// <summary>House number addition</summary>
    string? HouseNumberAddition,
    /// <summary>Street name</summary>
    string? Street,
    /// <summary>City name</summary>
    string? City,
    /// <summary>Country code (ISO 3166-1 alpha-2)</summary>
    string? Country,
    /// <summary>Connection status</summary>
    string? Status,
    /// <summary>Associated grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a connection object
/// </summary>
public record CreateConnectionObjectRequest(
    string? Ean,
    string PostalCode,
    int HouseNumber,
    string? HouseNumberAddition,
    string? Street,
    string? City,
    string? Country,
    Guid? GridOperatorId
);

/// <summary>
/// Request model for updating a connection object
/// </summary>
public record UpdateConnectionObjectRequest(
    string? Ean,
    string? PostalCode,
    int? HouseNumber,
    string? HouseNumberAddition,
    string? Street,
    string? City,
    string? Country,
    string? Status,
    Guid? GridOperatorId
);
