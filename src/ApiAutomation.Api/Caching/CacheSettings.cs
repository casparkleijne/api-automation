namespace ApiAutomation.Api.Caching;

/// <summary>
/// Cache duration settings
/// </summary>
public class CacheSettings
{
    public const string SectionName = "Caching";

    /// <summary>
    /// Default cache duration in minutes
    /// </summary>
    public int DefaultDurationMinutes { get; set; } = 5;

    /// <summary>
    /// Cache duration for list/collection queries in minutes
    /// </summary>
    public int ListDurationMinutes { get; set; } = 2;

    /// <summary>
    /// Cache duration for single item queries in minutes
    /// </summary>
    public int ItemDurationMinutes { get; set; } = 5;

    /// <summary>
    /// Cache duration for count queries in minutes
    /// </summary>
    public int CountDurationMinutes { get; set; } = 1;

    /// <summary>
    /// Enable sliding expiration (resets on access)
    /// </summary>
    public bool UseSlidingExpiration { get; set; } = true;
}
