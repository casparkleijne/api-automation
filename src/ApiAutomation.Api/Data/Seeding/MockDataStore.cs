using ApiAutomation.Api.Data.Entities;

namespace ApiAutomation.Api.Data.Seeding;

/// <summary>
/// In-memory data store with mock data (Singleton)
/// </summary>
public class MockDataStore
{
    public List<QuestionEntity> Questions { get; } = new();
    public List<AnswerEntity> Answers { get; } = new();

    public MockDataStore()
    {
        SeedQuestions();
        SeedAnswers();
    }

    private void SeedQuestions()
    {
        Questions.AddRange(new[]
        {
            new QuestionEntity
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777771"),
                Name = "Connection Capacity",
                Description = "Question about connection capacity",
                Code = "Q001",
                Priority = 1,
                QuestionTitle = "Connection Capacity",
                QuestionText = "What is the requested connection capacity?",
                ResultFormat = "select",
                Placeholder = "Select capacity",
                HasAction = false,
                IsEnabled = true,
                AnswerHandler = 1,
                IsMandatory = true,
                ShowForAddressableObjects = true,
                ShowForNonAddressableObjects = false,
                ShowForAddressableObjectsOnly = false,
                IsActive = true,
                StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2099, 12, 31, 23, 59, 59, DateTimeKind.Utc)
            },
            new QuestionEntity
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777772"),
                Name = "Installation Date",
                Description = "Question about preferred installation date",
                Code = "Q002",
                Priority = 2,
                QuestionTitle = "Installation Date",
                QuestionText = "What is the preferred installation date?",
                ResultFormat = "date",
                Placeholder = "Select date",
                HasAction = false,
                IsEnabled = true,
                AnswerHandler = 2,
                IsMandatory = true,
                ShowForAddressableObjects = true,
                ShowForNonAddressableObjects = true,
                ShowForAddressableObjectsOnly = false,
                IsActive = true,
                StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2099, 12, 31, 23, 59, 59, DateTimeKind.Utc)
            }
        });
    }

    private void SeedAnswers()
    {
        var questionId = Guid.Parse("77777777-7777-7777-7777-777777777771");

        Answers.AddRange(new[]
        {
            new AnswerEntity
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888881"),
                Code = "A001_1",
                Text = "1x25A (Single phase)",
                Description = "Suitable for small apartments",
                QuestionId = questionId,
                DisplayOrder = 1,
                IsDefault = false,
                IsActive = true
            },
            new AnswerEntity
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888882"),
                Code = "A001_2",
                Text = "3x25A (Three phase)",
                Description = "Standard for most homes",
                QuestionId = questionId,
                DisplayOrder = 2,
                IsDefault = true,
                IsActive = true
            },
            new AnswerEntity
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888883"),
                Code = "A001_3",
                Text = "3x35A (Three phase)",
                Description = "For homes with heat pump or EV charger",
                QuestionId = questionId,
                DisplayOrder = 3,
                IsDefault = false,
                IsActive = true
            }
        });
    }
}
