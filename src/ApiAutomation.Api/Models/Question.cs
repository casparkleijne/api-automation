using ApiAutomation.Api.Repositories;

namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a question (Vraag) in a questionnaire or form
/// </summary>
public record Question(
    Guid Id,
    string? Name,
    string? Description,
    string? WebsiteLink,
    string? WebsiteText,
    string? Placeholder,
    string? Code,
    int? Priority,
    string? QuestionTitle,
    string? QuestionText,
    string? ResultFormat,
    bool HasAction,
    bool IsEnabled,
    int? AnswerHandler,
    bool IsMandatory,
    Guid? AnswerTypeId,
    bool ShowForAddressableObjects,
    bool ShowForNonAddressableObjects,
    bool ShowForAddressableObjectsOnly,
    Guid? QuestionRequestTypeId,
    bool IsActive,
    string StartDate,
    string EndDate
) : IEntity, ICodeEntity, IActivatable;

/// <summary>
/// Represents an answer type for questions
/// </summary>
public record AnswerType(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Answer type name</summary>
    string? Name,
    /// <summary>Description of the answer type</summary>
    string? Description,
    /// <summary>Whether the answer is quantifiable</summary>
    bool IsQuantifiable,
    /// <summary>Whether the answer type is enabled</summary>
    bool IsEnabled,
    /// <summary>Whether the answer is processable</summary>
    bool IsProcessable
);

/// <summary>
/// Request model for creating a question
/// </summary>
public record CreateQuestionRequest(
    string? Name,
    string? Description,
    string? WebsiteLink,
    string? WebsiteText,
    string? Placeholder,
    string? Code,
    int? Priority,
    string? QuestionTitle,
    string? QuestionText,
    string? ResultFormat,
    bool HasAction,
    bool IsEnabled,
    int? AnswerHandler,
    bool IsMandatory,
    Guid? AnswerTypeId,
    bool ShowForAddressableObjects,
    bool ShowForNonAddressableObjects,
    bool ShowForAddressableObjectsOnly,
    Guid? QuestionRequestTypeId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a question
/// </summary>
public record UpdateQuestionRequest(
    string? Name,
    string? Description,
    string? WebsiteLink,
    string? WebsiteText,
    string? Placeholder,
    string? Code,
    int? Priority,
    string? QuestionTitle,
    string? QuestionText,
    string? ResultFormat,
    bool? HasAction,
    bool? IsEnabled,
    int? AnswerHandler,
    bool? IsMandatory,
    Guid? AnswerTypeId,
    bool? ShowForAddressableObjects,
    bool? ShowForNonAddressableObjects,
    bool? ShowForAddressableObjectsOnly,
    Guid? QuestionRequestTypeId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Request model for creating an answer type
/// </summary>
public record CreateAnswerTypeRequest(
    string? Name,
    string? Description,
    bool IsQuantifiable,
    bool IsEnabled,
    bool IsProcessable
);

/// <summary>
/// Request model for updating an answer type
/// </summary>
public record UpdateAnswerTypeRequest(
    string? Name,
    string? Description,
    bool? IsQuantifiable,
    bool? IsEnabled,
    bool? IsProcessable
);

/// <summary>
/// Represents question information/help text
/// </summary>
public record QuestionInfo(
    Guid Id,
    string? InfoTitle,
    string? InfoText,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents an action for a question based on conditions
/// </summary>
public record QuestionAction(
    Guid Id,
    Guid? QuestionId,
    Guid? ActionConditionId,
    string? AnswerResult,
    Guid? QuestionInfoId,
    int? QuestionInfoPositionId,
    Guid? ProcessVariantId,
    int? MinimalWaitTime,
    bool IsRelative,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a condition for triggering actions
/// </summary>
public record ActionCondition(
    Guid Id,
    string? Name,
    string? Description,
    string? ConditionType,
    string? ConditionValue,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a relation between questions
/// </summary>
public record QuestionRelation(
    Guid Id,
    string? PrimaryRelationType,
    string? SecondaryRelationType,
    Guid? QuestionId,
    Guid? GridOperatorId,
    Guid? PrimaryRelationId,
    Guid? SecondaryRelationId,
    string? Name,
    string? Description,
    int? Priority,
    bool EanCheck,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a question request type
/// </summary>
public record QuestionRequestType(
    Guid Id,
    string? Name,
    string? Description,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents an answer option for a question (Antwoord)
/// </summary>
public record Answer(
    Guid Id,
    string? Code,
    string? Text,
    string? Description,
    Guid QuestionId,
    int DisplayOrder,
    bool IsDefault,
    bool IsActive,
    string CreatedAt,
    string? ModifiedAt
) : IEntity, ICodeEntity, IActivatable;

/// <summary>
/// Request model for creating an answer
/// </summary>
public record CreateAnswerRequest(
    string? Code,
    string Text,
    string? Description,
    Guid QuestionId,
    int DisplayOrder,
    bool IsDefault
);

/// <summary>
/// Request model for updating an answer
/// </summary>
public record UpdateAnswerRequest(
    string? Code,
    string? Text,
    string? Description,
    int? DisplayOrder,
    bool? IsDefault,
    bool? IsActive
);
