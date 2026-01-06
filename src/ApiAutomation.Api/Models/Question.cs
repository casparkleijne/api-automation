namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a question (Vraag) in a questionnaire or form
/// </summary>
public record Question(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Question name</summary>
    string? Name,
    /// <summary>Description of the question</summary>
    string? Description,
    /// <summary>Website link for more information</summary>
    string? WebsiteLink,
    /// <summary>Website link text</summary>
    string? WebsiteText,
    /// <summary>Placeholder text for input</summary>
    string? Placeholder,
    /// <summary>Question code</summary>
    string? Code,
    /// <summary>Priority for ordering</summary>
    int? Priority,
    /// <summary>Question title for display</summary>
    string? QuestionTitle,
    /// <summary>Question text</summary>
    string? QuestionText,
    /// <summary>Result format specification</summary>
    string? ResultFormat,
    /// <summary>Whether the question has an action</summary>
    bool HasAction,
    /// <summary>Whether the question is enabled</summary>
    bool IsEnabled,
    /// <summary>Answer handler type</summary>
    int? AnswerHandler,
    /// <summary>Whether the question is mandatory</summary>
    bool IsMandatory,
    /// <summary>Answer type ID</summary>
    Guid? AnswerTypeId,
    /// <summary>Show for addressable objects</summary>
    bool ShowForAddressableObjects,
    /// <summary>Show for non-addressable objects</summary>
    bool ShowForNonAddressableObjects,
    /// <summary>Show for addressable objects only</summary>
    bool ShowForAddressableObjectsOnly,
    /// <summary>Question request type ID</summary>
    Guid? QuestionRequestTypeId,
    /// <summary>Whether the question is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

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
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Answer code</summary>
    string? Code,
    /// <summary>Answer text</summary>
    string? Text,
    /// <summary>Answer description</summary>
    string? Description,
    /// <summary>Question ID this answer belongs to</summary>
    Guid QuestionId,
    /// <summary>Display order</summary>
    int DisplayOrder,
    /// <summary>Whether this is the default answer</summary>
    bool IsDefault,
    /// <summary>Whether this answer is active</summary>
    bool IsActive,
    /// <summary>Created timestamp in ISO 8601 format</summary>
    string CreatedAt,
    /// <summary>Modified timestamp in ISO 8601 format</summary>
    string? ModifiedAt
);

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
