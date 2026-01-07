using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ApiAutomation.Api.Models;
using ApiAutomation.Api.Repositories;

namespace ApiAutomation.Api.Caching;

/// <summary>
/// Cached decorator for Question repository
/// </summary>
public class CachedQuestionRepository : IQuestionRepository
{
    private readonly IQuestionRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _settings;
    private const string Prefix = CacheKeys.Questions;

    public CachedQuestionRepository(
        IQuestionRepository inner,
        IMemoryCache cache,
        IOptions<CacheSettings> settings)
    {
        _inner = inner;
        _cache = cache;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        var cacheKey = CacheKeys.All(Prefix);

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Question>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetAllAsync()).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<Question?> GetByIdAsync(Guid id)
    {
        var cacheKey = CacheKeys.ById(Prefix, id);

        if (_cache.TryGetValue(cacheKey, out Question? cached))
            return cached;

        var result = await _inner.GetByIdAsync(id);
        if (result != null)
            _cache.Set(cacheKey, result, GetItemOptions());

        return result;
    }

    public async Task<Question?> GetByCodeAsync(string code)
    {
        var cacheKey = CacheKeys.ByCode(Prefix, code);

        if (_cache.TryGetValue(cacheKey, out Question? cached))
            return cached;

        var result = await _inner.GetByCodeAsync(code);
        if (result != null)
            _cache.Set(cacheKey, result, GetItemOptions());

        return result;
    }

    public async Task<IEnumerable<Question>> GetByAnswerTypeIdAsync(Guid answerTypeId)
    {
        var cacheKey = CacheKeys.ByParent(Prefix, "answerType", answerTypeId);

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Question>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetByAnswerTypeIdAsync(answerTypeId)).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<IEnumerable<Question>> GetMandatoryAsync()
    {
        var cacheKey = $"{Prefix}:mandatory";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Question>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetMandatoryAsync()).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<IEnumerable<Question>> GetActiveAsync()
    {
        var cacheKey = $"{Prefix}:active";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Question>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetActiveAsync()).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<int> CountAsync()
    {
        var cacheKey = CacheKeys.Count(Prefix);

        if (_cache.TryGetValue(cacheKey, out int cached))
            return cached;

        var result = await _inner.CountAsync();
        _cache.Set(cacheKey, result, GetCountOptions());
        return result;
    }

    public async Task<Question> CreateAsync(CreateQuestionRequest request)
    {
        var result = await _inner.CreateAsync(request);
        InvalidateAll();
        return result;
    }

    public async Task<Question?> UpdateAsync(Guid id, UpdateQuestionRequest request)
    {
        var result = await _inner.UpdateAsync(id, request);
        if (result != null)
        {
            InvalidateAll();
            _cache.Remove(CacheKeys.ById(Prefix, id));
            if (result.Code != null)
                _cache.Remove(CacheKeys.ByCode(Prefix, result.Code));
        }
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Get item first to invalidate code cache
        var item = await _inner.GetByIdAsync(id);
        var result = await _inner.DeleteAsync(id);

        if (result)
        {
            InvalidateAll();
            _cache.Remove(CacheKeys.ById(Prefix, id));
            if (item?.Code != null)
                _cache.Remove(CacheKeys.ByCode(Prefix, item.Code));
        }

        return result;
    }

    private void InvalidateAll()
    {
        _cache.Remove(CacheKeys.All(Prefix));
        _cache.Remove(CacheKeys.Count(Prefix));
        _cache.Remove($"{Prefix}:mandatory");
        _cache.Remove($"{Prefix}:active");
    }

    private MemoryCacheEntryOptions GetListOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes / 2.0));

    private MemoryCacheEntryOptions GetItemOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes / 2.0));

    private MemoryCacheEntryOptions GetCountOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CountDurationMinutes));
}

/// <summary>
/// Cached decorator for Answer repository
/// </summary>
public class CachedAnswerRepository : IAnswerRepository
{
    private readonly IAnswerRepository _inner;
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _settings;
    private const string Prefix = CacheKeys.Answers;

    public CachedAnswerRepository(
        IAnswerRepository inner,
        IMemoryCache cache,
        IOptions<CacheSettings> settings)
    {
        _inner = inner;
        _cache = cache;
        _settings = settings.Value;
    }

    public async Task<IEnumerable<Answer>> GetAllAsync()
    {
        var cacheKey = CacheKeys.All(Prefix);

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Answer>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetAllAsync()).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<Answer?> GetByIdAsync(Guid id)
    {
        var cacheKey = CacheKeys.ById(Prefix, id);

        if (_cache.TryGetValue(cacheKey, out Answer? cached))
            return cached;

        var result = await _inner.GetByIdAsync(id);
        if (result != null)
            _cache.Set(cacheKey, result, GetItemOptions());

        return result;
    }

    public async Task<Answer?> GetByCodeAsync(string code)
    {
        var cacheKey = CacheKeys.ByCode(Prefix, code);

        if (_cache.TryGetValue(cacheKey, out Answer? cached))
            return cached;

        var result = await _inner.GetByCodeAsync(code);
        if (result != null)
            _cache.Set(cacheKey, result, GetItemOptions());

        return result;
    }

    public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId)
    {
        var cacheKey = CacheKeys.ByParent(Prefix, "question", questionId);

        if (_cache.TryGetValue(cacheKey, out IEnumerable<Answer>? cached) && cached != null)
            return cached;

        var result = (await _inner.GetByQuestionIdAsync(questionId)).ToList();
        _cache.Set(cacheKey, result, GetListOptions());
        return result;
    }

    public async Task<int> CountAsync()
    {
        var cacheKey = CacheKeys.Count(Prefix);

        if (_cache.TryGetValue(cacheKey, out int cached))
            return cached;

        var result = await _inner.CountAsync();
        _cache.Set(cacheKey, result, GetCountOptions());
        return result;
    }

    public async Task<Answer> CreateAsync(CreateAnswerRequest request)
    {
        var result = await _inner.CreateAsync(request);
        InvalidateAll();
        // Invalidate parent question's answers cache
        _cache.Remove(CacheKeys.ByParent(Prefix, "question", result.QuestionId));
        return result;
    }

    public async Task<Answer?> UpdateAsync(Guid id, UpdateAnswerRequest request)
    {
        var result = await _inner.UpdateAsync(id, request);
        if (result != null)
        {
            InvalidateAll();
            _cache.Remove(CacheKeys.ById(Prefix, id));
            _cache.Remove(CacheKeys.ByParent(Prefix, "question", result.QuestionId));
            if (result.Code != null)
                _cache.Remove(CacheKeys.ByCode(Prefix, result.Code));
        }
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _inner.GetByIdAsync(id);
        var result = await _inner.DeleteAsync(id);

        if (result)
        {
            InvalidateAll();
            _cache.Remove(CacheKeys.ById(Prefix, id));
            if (item != null)
            {
                _cache.Remove(CacheKeys.ByParent(Prefix, "question", item.QuestionId));
                if (item.Code != null)
                    _cache.Remove(CacheKeys.ByCode(Prefix, item.Code));
            }
        }

        return result;
    }

    private void InvalidateAll()
    {
        _cache.Remove(CacheKeys.All(Prefix));
        _cache.Remove(CacheKeys.Count(Prefix));
    }

    private MemoryCacheEntryOptions GetListOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ListDurationMinutes / 2.0));

    private MemoryCacheEntryOptions GetItemOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes))
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.ItemDurationMinutes / 2.0));

    private MemoryCacheEntryOptions GetCountOptions() =>
        new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.CountDurationMinutes));
}
