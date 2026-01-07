using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

public class InMemoryQuestionRepository : IQuestionRepository
{
    private readonly List<Question> _items = new()
    {
        new Question(
            Id: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            Name: "Connection Capacity",
            Description: "Question about connection capacity",
            WebsiteLink: null,
            WebsiteText: null,
            Placeholder: "Select capacity",
            Code: "Q001",
            Priority: 1,
            QuestionTitle: "Connection Capacity",
            QuestionText: "What is the requested connection capacity?",
            ResultFormat: "select",
            HasAction: false,
            IsEnabled: true,
            AnswerHandler: 1,
            IsMandatory: true,
            AnswerTypeId: null,
            ShowForAddressableObjects: true,
            ShowForNonAddressableObjects: false,
            ShowForAddressableObjectsOnly: false,
            QuestionRequestTypeId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Question(
            Id: Guid.Parse("77777777-7777-7777-7777-777777777772"),
            Name: "Installation Date",
            Description: "Question about preferred installation date",
            WebsiteLink: null,
            WebsiteText: null,
            Placeholder: "Select date",
            Code: "Q002",
            Priority: 2,
            QuestionTitle: "Installation Date",
            QuestionText: "What is the preferred installation date?",
            ResultFormat: "date",
            HasAction: false,
            IsEnabled: true,
            AnswerHandler: 2,
            IsMandatory: true,
            AnswerTypeId: null,
            ShowForAddressableObjects: true,
            ShowForNonAddressableObjects: true,
            ShowForAddressableObjectsOnly: false,
            QuestionRequestTypeId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    public Task<IEnumerable<Question>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Question?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<Question?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Code == code));

    public Task<IEnumerable<Question>> GetByAnswerTypeIdAsync(Guid answerTypeId) =>
        Task.FromResult(_items.Where(x => x.AnswerTypeId == answerTypeId).AsEnumerable());

    public Task<IEnumerable<Question>> GetMandatoryAsync() =>
        Task.FromResult(_items.Where(x => x.IsMandatory).AsEnumerable());

    public Task<IEnumerable<Question>> GetActiveAsync() =>
        Task.FromResult(_items.Where(x => x.IsActive).AsEnumerable());

    public Task<Question> CreateAsync(CreateQuestionRequest request)
    {
        var item = new Question(
            Id: Guid.NewGuid(),
            Name: request.Name,
            Description: request.Description,
            WebsiteLink: request.WebsiteLink,
            WebsiteText: request.WebsiteText,
            Placeholder: request.Placeholder,
            Code: request.Code,
            Priority: request.Priority,
            QuestionTitle: request.QuestionTitle,
            QuestionText: request.QuestionText,
            ResultFormat: request.ResultFormat,
            HasAction: request.HasAction,
            IsEnabled: request.IsEnabled,
            AnswerHandler: request.AnswerHandler,
            IsMandatory: request.IsMandatory,
            AnswerTypeId: request.AnswerTypeId,
            ShowForAddressableObjects: request.ShowForAddressableObjects,
            ShowForNonAddressableObjects: request.ShowForNonAddressableObjects,
            ShowForAddressableObjectsOnly: request.ShowForAddressableObjectsOnly,
            QuestionRequestTypeId: request.QuestionRequestTypeId,
            IsActive: true,
            StartDate: request.StartDate,
            EndDate: request.EndDate
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Question?> UpdateAsync(Guid id, UpdateQuestionRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<Question?>(null);

        var existing = _items[index];
        var updated = existing with
        {
            Name = request.Name ?? existing.Name,
            Description = request.Description ?? existing.Description,
            WebsiteLink = request.WebsiteLink ?? existing.WebsiteLink,
            WebsiteText = request.WebsiteText ?? existing.WebsiteText,
            Placeholder = request.Placeholder ?? existing.Placeholder,
            Code = request.Code ?? existing.Code,
            Priority = request.Priority ?? existing.Priority,
            QuestionTitle = request.QuestionTitle ?? existing.QuestionTitle,
            QuestionText = request.QuestionText ?? existing.QuestionText,
            ResultFormat = request.ResultFormat ?? existing.ResultFormat,
            HasAction = request.HasAction ?? existing.HasAction,
            IsEnabled = request.IsEnabled ?? existing.IsEnabled,
            AnswerHandler = request.AnswerHandler ?? existing.AnswerHandler,
            IsMandatory = request.IsMandatory ?? existing.IsMandatory,
            AnswerTypeId = request.AnswerTypeId ?? existing.AnswerTypeId,
            ShowForAddressableObjects = request.ShowForAddressableObjects ?? existing.ShowForAddressableObjects,
            ShowForNonAddressableObjects = request.ShowForNonAddressableObjects ?? existing.ShowForNonAddressableObjects,
            ShowForAddressableObjectsOnly = request.ShowForAddressableObjectsOnly ?? existing.ShowForAddressableObjectsOnly,
            QuestionRequestTypeId = request.QuestionRequestTypeId ?? existing.QuestionRequestTypeId,
            IsActive = request.IsActive ?? existing.IsActive,
            StartDate = request.StartDate ?? existing.StartDate,
            EndDate = request.EndDate ?? existing.EndDate
        };
        _items[index] = updated;
        return Task.FromResult<Question?>(updated);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult(false);
        _items.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<int> CountAsync() => Task.FromResult(_items.Count);
}

public class InMemoryAnswerRepository : IAnswerRepository
{
    private readonly List<Answer> _items = new()
    {
        new Answer(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888881"),
            Code: "A001_1",
            Text: "1x25A (Single phase)",
            Description: "Suitable for small apartments",
            QuestionId: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            DisplayOrder: 1,
            IsDefault: false,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Answer(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888882"),
            Code: "A001_2",
            Text: "3x25A (Three phase)",
            Description: "Standard for most homes",
            QuestionId: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            DisplayOrder: 2,
            IsDefault: true,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Answer(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888883"),
            Code: "A001_3",
            Text: "3x35A (Three phase)",
            Description: "For homes with heat pump or EV charger",
            QuestionId: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            DisplayOrder: 3,
            IsDefault: false,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public Task<IEnumerable<Answer>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Answer?> GetByIdAsync(Guid id) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task<Answer?> GetByCodeAsync(string code) =>
        Task.FromResult(_items.FirstOrDefault(x => x.Code == code));

    public Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId) =>
        Task.FromResult(_items.Where(x => x.QuestionId == questionId).OrderBy(x => x.DisplayOrder).AsEnumerable());

    public Task<Answer> CreateAsync(CreateAnswerRequest request)
    {
        var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        var item = new Answer(
            Id: Guid.NewGuid(),
            Code: request.Code,
            Text: request.Text,
            Description: request.Description,
            QuestionId: request.QuestionId,
            DisplayOrder: request.DisplayOrder,
            IsDefault: request.IsDefault,
            IsActive: true,
            CreatedAt: now,
            ModifiedAt: null
        );
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Answer?> UpdateAsync(Guid id, UpdateAnswerRequest request)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult<Answer?>(null);

        var existing = _items[index];
        var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        var updated = existing with
        {
            Code = request.Code ?? existing.Code,
            Text = request.Text ?? existing.Text,
            Description = request.Description ?? existing.Description,
            DisplayOrder = request.DisplayOrder ?? existing.DisplayOrder,
            IsDefault = request.IsDefault ?? existing.IsDefault,
            IsActive = request.IsActive ?? existing.IsActive,
            ModifiedAt = now
        };
        _items[index] = updated;
        return Task.FromResult<Answer?>(updated);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0) return Task.FromResult(false);
        _items.RemoveAt(index);
        return Task.FromResult(true);
    }

    public Task<int> CountAsync() => Task.FromResult(_items.Count);
}
