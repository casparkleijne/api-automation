using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories.InMemory;

/// <summary>
/// In-memory Question repository using generic base class
/// </summary>
public class InMemoryQuestionRepository
    : InMemoryCodeRepositoryBase<Question, CreateQuestionRequest, UpdateQuestionRequest>,
      IQuestionRepository
{
    public InMemoryQuestionRepository() => SeedData();

    // Domain-specific queries
    public Task<IEnumerable<Question>> GetByAnswerTypeIdAsync(Guid answerTypeId) =>
        Task.FromResult(Items.Where(x => x.AnswerTypeId == answerTypeId).AsEnumerable());

    public Task<IEnumerable<Question>> GetMandatoryAsync() =>
        Task.FromResult(Items.Where(x => x.IsMandatory).AsEnumerable());

    public Task<IEnumerable<Question>> GetActiveAsync() =>
        Task.FromResult(Items.Where(x => x.IsActive).AsEnumerable());

    // Mapping logic (Single Responsibility)
    protected override Question MapToEntity(CreateQuestionRequest r) => new(
        Id: Guid.NewGuid(),
        Name: r.Name,
        Description: r.Description,
        WebsiteLink: r.WebsiteLink,
        WebsiteText: r.WebsiteText,
        Placeholder: r.Placeholder,
        Code: r.Code,
        Priority: r.Priority,
        QuestionTitle: r.QuestionTitle,
        QuestionText: r.QuestionText,
        ResultFormat: r.ResultFormat,
        HasAction: r.HasAction,
        IsEnabled: r.IsEnabled,
        AnswerHandler: r.AnswerHandler,
        IsMandatory: r.IsMandatory,
        AnswerTypeId: r.AnswerTypeId,
        ShowForAddressableObjects: r.ShowForAddressableObjects,
        ShowForNonAddressableObjects: r.ShowForNonAddressableObjects,
        ShowForAddressableObjectsOnly: r.ShowForAddressableObjectsOnly,
        QuestionRequestTypeId: r.QuestionRequestTypeId,
        IsActive: true,
        StartDate: r.StartDate,
        EndDate: r.EndDate
    );

    protected override Question ApplyUpdate(Question e, UpdateQuestionRequest r) => e with
    {
        Name = r.Name ?? e.Name,
        Description = r.Description ?? e.Description,
        WebsiteLink = r.WebsiteLink ?? e.WebsiteLink,
        WebsiteText = r.WebsiteText ?? e.WebsiteText,
        Placeholder = r.Placeholder ?? e.Placeholder,
        Code = r.Code ?? e.Code,
        Priority = r.Priority ?? e.Priority,
        QuestionTitle = r.QuestionTitle ?? e.QuestionTitle,
        QuestionText = r.QuestionText ?? e.QuestionText,
        ResultFormat = r.ResultFormat ?? e.ResultFormat,
        HasAction = r.HasAction ?? e.HasAction,
        IsEnabled = r.IsEnabled ?? e.IsEnabled,
        AnswerHandler = r.AnswerHandler ?? e.AnswerHandler,
        IsMandatory = r.IsMandatory ?? e.IsMandatory,
        AnswerTypeId = r.AnswerTypeId ?? e.AnswerTypeId,
        ShowForAddressableObjects = r.ShowForAddressableObjects ?? e.ShowForAddressableObjects,
        ShowForNonAddressableObjects = r.ShowForNonAddressableObjects ?? e.ShowForNonAddressableObjects,
        ShowForAddressableObjectsOnly = r.ShowForAddressableObjectsOnly ?? e.ShowForAddressableObjectsOnly,
        QuestionRequestTypeId = r.QuestionRequestTypeId ?? e.QuestionRequestTypeId,
        IsActive = r.IsActive ?? e.IsActive,
        StartDate = r.StartDate ?? e.StartDate,
        EndDate = r.EndDate ?? e.EndDate
    };

    private void SeedData() => Seed(
        new Question(Guid.Parse("77777777-7777-7777-7777-777777777771"),
            "Connection Capacity", "Question about connection capacity", null, null,
            "Select capacity", "Q001", 1, "Connection Capacity",
            "What is the requested connection capacity?", "select", false, true, 1, true,
            null, true, false, false, null, true,
            "2025-01-01T00:00:00.000Z", "2099-12-31T23:59:59.999Z"),
        new Question(Guid.Parse("77777777-7777-7777-7777-777777777772"),
            "Installation Date", "Question about preferred installation date", null, null,
            "Select date", "Q002", 2, "Installation Date",
            "What is the preferred installation date?", "date", false, true, 2, true,
            null, true, true, false, null, true,
            "2025-01-01T00:00:00.000Z", "2099-12-31T23:59:59.999Z")
    );
}

/// <summary>
/// In-memory Answer repository using generic base class
/// </summary>
public class InMemoryAnswerRepository
    : InMemoryCodeRepositoryBase<Answer, CreateAnswerRequest, UpdateAnswerRequest>,
      IAnswerRepository
{
    public InMemoryAnswerRepository() => SeedData();

    // Domain-specific query
    public Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId) =>
        Task.FromResult(Items.Where(x => x.QuestionId == questionId)
            .OrderBy(x => x.DisplayOrder).AsEnumerable());

    protected override Answer MapToEntity(CreateAnswerRequest r) => new(
        Id: Guid.NewGuid(),
        Code: r.Code,
        Text: r.Text,
        Description: r.Description,
        QuestionId: r.QuestionId,
        DisplayOrder: r.DisplayOrder,
        IsDefault: r.IsDefault,
        IsActive: true,
        CreatedAt: DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
        ModifiedAt: null
    );

    protected override Answer ApplyUpdate(Answer e, UpdateAnswerRequest r) => e with
    {
        Code = r.Code ?? e.Code,
        Text = r.Text ?? e.Text,
        Description = r.Description ?? e.Description,
        DisplayOrder = r.DisplayOrder ?? e.DisplayOrder,
        IsDefault = r.IsDefault ?? e.IsDefault,
        IsActive = r.IsActive ?? e.IsActive,
        ModifiedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    };

    private void SeedData() => Seed(
        new Answer(Guid.Parse("88888888-8888-8888-8888-888888888881"),
            "A001_1", "1x25A (Single phase)", "Suitable for small apartments",
            Guid.Parse("77777777-7777-7777-7777-777777777771"), 1, false, true,
            "2025-01-01T00:00:00.000Z", null),
        new Answer(Guid.Parse("88888888-8888-8888-8888-888888888882"),
            "A001_2", "3x25A (Three phase)", "Standard for most homes",
            Guid.Parse("77777777-7777-7777-7777-777777777771"), 2, true, true,
            "2025-01-01T00:00:00.000Z", null),
        new Answer(Guid.Parse("88888888-8888-8888-8888-888888888883"),
            "A001_3", "3x35A (Three phase)", "For homes with heat pump or EV charger",
            Guid.Parse("77777777-7777-7777-7777-777777777771"), 3, false, true,
            "2025-01-01T00:00:00.000Z", null)
    );
}
