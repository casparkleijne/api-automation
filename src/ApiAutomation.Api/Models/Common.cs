namespace ApiAutomation.Api.Models;

/// <summary>
/// Paginated response wrapper for list endpoints
/// </summary>
/// <typeparam name="T">Type of items in the response</typeparam>
public record PaginatedResponse<T>(
    /// <summary>List of items</summary>
    IReadOnlyList<T> Items,
    /// <summary>Current page number (1-based)</summary>
    int Page,
    /// <summary>Number of items per page</summary>
    int PageSize,
    /// <summary>Total number of items</summary>
    int TotalCount,
    /// <summary>Total number of pages</summary>
    int TotalPages
);

/// <summary>
/// Common query parameters for list endpoints
/// </summary>
public record PaginationQuery
{
    /// <summary>Page number (1-based, default: 1)</summary>
    public int Page { get; init; } = 1;
    /// <summary>Number of items per page (default: 20, max: 100)</summary>
    public int PageSize { get; init; } = 20;
    /// <summary>Sort field</summary>
    public string? SortBy { get; init; }
    /// <summary>Sort direction (asc, desc)</summary>
    public string? SortDirection { get; init; }
}

/// <summary>
/// Health check response
/// </summary>
public record HealthResponse(
    /// <summary>Health status</summary>
    string Status,
    /// <summary>Timestamp in ISO 8601 UTC format</summary>
    string Timestamp,
    /// <summary>API version</summary>
    string Version
);

/// <summary>
/// Secure endpoint response
/// </summary>
public record SecureResponse(
    /// <summary>Response message</summary>
    string Message,
    /// <summary>User name from token</summary>
    string UserName,
    /// <summary>Object ID (oid) from token</summary>
    string ObjectId,
    /// <summary>Timestamp in ISO 8601 UTC format</summary>
    string Timestamp
);
