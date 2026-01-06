namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a question (Vraag) in a questionnaire or form
/// </summary>
public record Question(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Question code</summary>
    string Code,
    /// <summary>Question text</summary>
    string Text,
    /// <summary>Help text for the question</summary>
    string? HelpText,
    /// <summary>Question type (text, number, select, multiselect, date, boolean)</summary>
    string QuestionType,
    /// <summary>Whether the question is required</summary>
    bool IsRequired,
    /// <summary>Display order</summary>
    int DisplayOrder,
    /// <summary>Associated product ID</summary>
    Guid? ProductId,
    /// <summary>Associated service ID</summary>
    Guid? ServiceId,
    /// <summary>Whether the question is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Represents an answer (Antwoord) option for a question
/// </summary>
public record Answer(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Answer code</summary>
    string Code,
    /// <summary>Answer text</summary>
    string Text,
    /// <summary>Description of the answer</summary>
    string? Description,
    /// <summary>Associated question ID</summary>
    Guid QuestionId,
    /// <summary>Display order</summary>
    int DisplayOrder,
    /// <summary>Whether this is the default answer</summary>
    bool IsDefault,
    /// <summary>Whether the answer is active</summary>
    bool IsActive,
    /// <summary>Creation timestamp in ISO 8601 UTC format</summary>
    string CreatedAt,
    /// <summary>Last modification timestamp in ISO 8601 UTC format</summary>
    string? ModifiedAt
);

/// <summary>
/// Request model for creating a question
/// </summary>
public record CreateQuestionRequest(
    string Code,
    string Text,
    string? HelpText,
    string QuestionType,
    bool IsRequired,
    int DisplayOrder,
    Guid? ProductId,
    Guid? ServiceId
);

/// <summary>
/// Request model for updating a question
/// </summary>
public record UpdateQuestionRequest(
    string? Code,
    string? Text,
    string? HelpText,
    string? QuestionType,
    bool? IsRequired,
    int? DisplayOrder,
    Guid? ProductId,
    Guid? ServiceId,
    bool? IsActive
);

/// <summary>
/// Request model for creating an answer
/// </summary>
public record CreateAnswerRequest(
    string Code,
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
