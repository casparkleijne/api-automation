using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ApiAutomation.Api.Repositories;

namespace ApiAutomation.Api.Caching;

/// <summary>
/// Generic caching decorator for repositories (Decorator pattern)
/// </summary>
public class CachedRepositoryDecorator<T, TCreate, TUpdate> : IRepository<T, TCreate, TUpdate>
    where T : class
{
    private readonly IRepository<T, TCreate, TUpdate> _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _settings;
    private readonly string _cachePrefix;

    public CachedRepositoryDecorator(
        IRepository<T, TCreate, TUpdate> inner,
        IMemoryCache cache,
        IOptions<CacheSettings> settings,
        string cachePrefix)
    {
        _inner = inner;
        _cache = cache;
        _settings = settings.Value;
        _cachePrefix = cachePrefix;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var cacheKey = CacheKeys.All(_cachePrefix);

        if (_cache.TryGetValue(cacheKey, out IEnumerable<T>? cached) && cached != null)
        {
            return cached;
        }

        var result = await _inner.GetAllAsync();
        var items = result.ToList();

        _cache.Set(cacheKey, items, GetListCacheOptions());

        return items;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var cacheKey = CacheKeys.ById(_cachePrefix, id);

        if (_cache.TryGetValue(cacheKey, out T? cached))
        {
            return cached;
        }

        var result = await _inner.GetByIdAsync(id);

        if (result != null)
        {
            _cache.Set(cacheKey, result, GetItemCacheOptions());
        }

        return result;
    }

    public async Task<T> CreateAsync(TCreate request)
    {
        var result = await _inner.CreateAsync(request);
        InvalidateCache();
        return result;
    }

    public async Task<T?> UpdateAsync(Guid id, TUpdate request)
    {
        var result = await _inner.UpdateAsync(id, request);

        if (result != null)
        {
            InvalidateCache();
            InvalidateItemCache(id);
        }

        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _inner.DeleteAsync(id);

        if (result)
        {
            InvalidateCache();
            InvalidateItemCache(id);
        }

        return result;
    }

    public async Task<int> CountAsync()
    {
        var cacheKey = CacheKeys.Count(_cachePrefix);

        if (_cache.TryGetValue(cacheKey, out int cached))
        {
            return cached;
        }

        var result = await _inner.CountAsync();
        _cache.Set(cacheKey, result, GetCountCacheOptions());

        return result;
    }

    protected void InvalidateCache()
    {
        _cache.Remove(CacheKeys.All(_cachePrefix));
        _cache.Remove(CacheKeys.Count(_cachePrefix));
    }

    protected void InvalidateItemCache(Guid id)
    {
        _cache.Remove(CacheKeys.ById(_cachePrefix, id));
    }

    protected void InvalidateCodeCache(string code)
    {
        _cache.Remove(CacheKeys.ByCode(_cachePrefix, code));
    }

    private MemoryCacheEntryOptions GetListCacheOptions()
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes));

        if (_settings.UseSlidingExpiration)
        {
            options.SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes / 2.0));
        }

        return options;
    }

    private MemoryCacheEntryOptions GetItemCacheOptions()
    {
        var options = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes));

        if (_settings.UseSlidingExpiration)
        {
            options.SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes / 2.0));
        }

        return options;
    }

    private MemoryCacheEntryOptions GetCountCacheOptions()
    {
        return new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CountDurationMinutes));
    }
}

/// <summary>
/// Caching decorator for repositories with code-based lookup
/// </summary>
public class CachedCodeRepositoryDecorator<T, TCreate, TUpdate>
    : CachedRepositoryDecorator<T, TCreate, TUpdate>, ICodeRepository<T, TCreate, TUpdate>
    where T : class
{
    private readonly ICodeRepository<T, TCreate, TUpdate> _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _settings;
    private readonly string _cachePrefix;

    public CachedCodeRepositoryDecorator(
        ICodeRepository<T, TCreate, TUpdate> inner,
        IMemoryCache cache,
        IOptions<CacheSettings> settings,
        string cachePrefix)
        : base(inner, cache, settings, cachePrefix)
    {
        _inner = inner;
        _cache = cache;
        _settings = settings.Value;
        _cachePrefix = cachePrefix;
    }

    public async Task<T?> GetByCodeAsync(string code)
    {
        var cacheKey = CacheKeys.ByCode(_cachePrefix, code);

        if (_cache.TryGetValue(cacheKey, out T? cached))
        {
            return cached;
        }

        var result = await _inner.GetByCodeAsync(code);

        if (result != null)
        {
            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes));

            if (_settings.UseSlidingExpiration)
            {
                options.SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes / 2.0));
            }

            _cache.Set(cacheKey, result, options);
        }

        return result;
    }
}
