namespace ApiAutomation.Api.Caching;

/// <summary>
/// Centralized cache key management
/// </summary>
public static class CacheKeys
{
    // Cache key prefixes per entity type
    public const string Questions = "questions";
    public const string Answers = "answers";
    public const string ConnectionObjects = "connection-objects";
    public const string GridOperators = "grid-operators";
    public const string Disciplines = "disciplines";
    public const string Services = "services";
    public const string SubServices = "sub-services";
    public const string Products = "products";
    public const string PriceComponents = "price-components";

    // Key generators
    public static string All(string prefix) => $"{prefix}:all";
    public static string ById(string prefix, Guid id) => $"{prefix}:id:{id}";
    public static string ByCode(string prefix, string code) => $"{prefix}:code:{code}";
    public static string Count(string prefix) => $"{prefix}:count";
    public static string ByParent(string prefix, string parentType, Guid parentId) =>
        $"{prefix}:parent:{parentType}:{parentId}";
}
