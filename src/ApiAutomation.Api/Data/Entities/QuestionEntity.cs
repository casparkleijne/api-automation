namespace ApiAutomation.Api.Data.Entities;

/// <summary>
/// Question data entity (internal)
/// </summary>
public class QuestionEntity : ValidityEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? WebsiteLink { get; set; }
    public string? WebsiteText { get; set; }
    public string? Placeholder { get; set; }
    public int? Priority { get; set; }
    public string? QuestionTitle { get; set; }
    public string? QuestionText { get; set; }
    public string? ResultFormat { get; set; }
    public bool HasAction { get; set; }
    public bool IsEnabled { get; set; } = true;
    public int? AnswerHandler { get; set; }
    public bool IsMandatory { get; set; }
    public Guid? AnswerTypeId { get; set; }
    public bool ShowForAddressableObjects { get; set; }
    public bool ShowForNonAddressableObjects { get; set; }
    public bool ShowForAddressableObjectsOnly { get; set; }
    public Guid? QuestionRequestTypeId { get; set; }

    // Navigation
    public virtual ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
}

/// <summary>
/// Answer data entity (internal)
/// </summary>
public class AnswerEntity : CodeEntity
{
    public string? Text { get; set; }
    public string? Description { get; set; }
    public Guid QuestionId { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsDefault { get; set; }

    // Navigation
    public virtual QuestionEntity? Question { get; set; }
}
