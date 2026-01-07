using AutoMapper;
using ApiAutomation.Api.Data.Entities;
using ApiAutomation.Api.Data.Seeding;
using ApiAutomation.Api.Models;
using ApiAutomation.Api.Repositories;

namespace ApiAutomation.Api.Data.Repositories;

/// <summary>
/// Question repository - works with entities internally, exposes DTOs externally
/// </summary>
public class QuestionRepository : IQuestionRepository
{
    private readonly MockDataStore _store;
    private readonly IMapper _mapper;

    public QuestionRepository(MockDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    public Task<IEnumerable<Question>> GetAllAsync() =>
        Task.FromResult(_store.Questions.Select(MapToDto));

    public Task<Question?> GetByIdAsync(Guid id)
    {
        var entity = _store.Questions.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entity is null ? null : MapToDto(entity));
    }

    public Task<Question?> GetByCodeAsync(string code)
    {
        var entity = _store.Questions.FirstOrDefault(x =>
            x.Code?.Equals(code, StringComparison.OrdinalIgnoreCase) ?? false);
        return Task.FromResult(entity is null ? null : MapToDto(entity));
    }

    public Task<IEnumerable<Question>> GetByAnswerTypeIdAsync(Guid answerTypeId) =>
        Task.FromResult(_store.Questions
            .Where(x => x.AnswerTypeId == answerTypeId)
            .Select(MapToDto));

    public Task<IEnumerable<Question>> GetMandatoryAsync() =>
        Task.FromResult(_store.Questions
            .Where(x => x.IsMandatory)
            .Select(MapToDto));

    public Task<IEnumerable<Question>> GetActiveAsync() =>
        Task.FromResult(_store.Questions
            .Where(x => x.IsActive)
            .Select(MapToDto));

    public Task<Question> CreateAsync(CreateQuestionRequest request)
    {
        var entity = _mapper.Map<QuestionEntity>(request);
        _store.Questions.Add(entity);
        return Task.FromResult(MapToDto(entity));
    }

    public Task<Question?> UpdateAsync(Guid id, UpdateQuestionRequest request)
    {
        var entity = _store.Questions.FirstOrDefault(x => x.Id == id);
        if (entity is null) return Task.FromResult<Question?>(null);

        // Partial update via AutoMapper
        _mapper.Map(request, entity);
        entity.ModifiedAt = DateTime.UtcNow;

        return Task.FromResult<Question?>(MapToDto(entity));
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var entity = _store.Questions.FirstOrDefault(x => x.Id == id);
        if (entity is null) return Task.FromResult(false);
        _store.Questions.Remove(entity);
        return Task.FromResult(true);
    }

    public Task<int> CountAsync() =>
        Task.FromResult(_store.Questions.Count);

    // Private mapping method - entities never leave this class
    private Question MapToDto(QuestionEntity e) => _mapper.Map<Question>(e);
}

/// <summary>
/// Answer repository - works with entities internally, exposes DTOs externally
/// </summary>
public class AnswerRepository : IAnswerRepository
{
    private readonly MockDataStore _store;
    private readonly IMapper _mapper;

    public AnswerRepository(MockDataStore store, IMapper mapper)
    {
        _store = store;
        _mapper = mapper;
    }

    public Task<IEnumerable<Answer>> GetAllAsync() =>
        Task.FromResult(_store.Answers.Select(MapToDto));

    public Task<Answer?> GetByIdAsync(Guid id)
    {
        var entity = _store.Answers.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entity is null ? null : MapToDto(entity));
    }

    public Task<Answer?> GetByCodeAsync(string code)
    {
        var entity = _store.Answers.FirstOrDefault(x =>
            x.Code?.Equals(code, StringComparison.OrdinalIgnoreCase) ?? false);
        return Task.FromResult(entity is null ? null : MapToDto(entity));
    }

    public Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId) =>
        Task.FromResult(_store.Answers
            .Where(x => x.QuestionId == questionId)
            .OrderBy(x => x.DisplayOrder)
            .Select(MapToDto));

    public Task<Answer> CreateAsync(CreateAnswerRequest request)
    {
        var entity = _mapper.Map<AnswerEntity>(request);
        _store.Answers.Add(entity);
        return Task.FromResult(MapToDto(entity));
    }

    public Task<Answer?> UpdateAsync(Guid id, UpdateAnswerRequest request)
    {
        var entity = _store.Answers.FirstOrDefault(x => x.Id == id);
        if (entity is null) return Task.FromResult<Answer?>(null);

        _mapper.Map(request, entity);
        entity.ModifiedAt = DateTime.UtcNow;

        return Task.FromResult<Answer?>(MapToDto(entity));
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var entity = _store.Answers.FirstOrDefault(x => x.Id == id);
        if (entity is null) return Task.FromResult(false);
        _store.Answers.Remove(entity);
        return Task.FromResult(true);
    }

    public Task<int> CountAsync() =>
        Task.FromResult(_store.Answers.Count);

    private Answer MapToDto(AnswerEntity e) => _mapper.Map<Answer>(e);
}
