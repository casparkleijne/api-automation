using ApiAutomation.Api.Repositories;

namespace ApiAutomation.Api.Caching;

/// <summary>
/// Extension methods for registering caching services
/// </summary>
public static class CachingExtensions
{
    /// <summary>
    /// Add memory caching with configured settings
    /// </summary>
    public static IServiceCollection AddApiCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register memory cache
        services.AddMemoryCache();

        // Configure cache settings from appsettings.json
        services.Configure<CacheSettings>(
            configuration.GetSection(CacheSettings.SectionName));

        return services;
    }

    /// <summary>
    /// Decorate a repository with caching (Decorator pattern)
    /// </summary>
    public static IServiceCollection DecorateWithCaching<TInterface, TDecorator>(
        this IServiceCollection services)
        where TInterface : class
        where TDecorator : class, TInterface
    {
        // Find existing registration
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TInterface));

        if (descriptor == null)
            throw new InvalidOperationException(
                $"No registration found for {typeof(TInterface).Name}. " +
                "Register the inner implementation before decorating.");

        // Remove existing registration
        services.Remove(descriptor);

        // Re-add as inner implementation with a different key
        var innerType = descriptor.ImplementationType
            ?? throw new InvalidOperationException("Implementation type required for decoration");

        services.Add(new ServiceDescriptor(
            innerType,
            innerType,
            descriptor.Lifetime));

        // Add decorator that depends on inner implementation
        services.Add(new ServiceDescriptor(
            typeof(TInterface),
            sp => ActivatorUtilities.CreateInstance<TDecorator>(
                sp,
                sp.GetRequiredService(innerType)),
            descriptor.Lifetime));

        return services;
    }
}
