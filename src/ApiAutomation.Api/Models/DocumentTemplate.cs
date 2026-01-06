namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a document template for generating documents
/// </summary>
public record DocumentTemplate(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Grid operator ID</summary>
    Guid? GridOperatorId,
    /// <summary>Template name</summary>
    string? Name,
    /// <summary>Document header content</summary>
    string? DocumentHeader,
    /// <summary>Document footer content</summary>
    string? DocumentFooter,
    /// <summary>Document template type ID</summary>
    Guid? DocumentTemplateTypeId,
    /// <summary>Whether the template is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a document template
/// </summary>
public record CreateDocumentTemplateRequest(
    Guid? GridOperatorId,
    string? Name,
    string? DocumentHeader,
    string? DocumentFooter,
    Guid? DocumentTemplateTypeId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a document template
/// </summary>
public record UpdateDocumentTemplateRequest(
    Guid? GridOperatorId,
    string? Name,
    string? DocumentHeader,
    string? DocumentFooter,
    Guid? DocumentTemplateTypeId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a document template type
/// </summary>
public record DocumentTemplateType(
    Guid Id,
    string? Name,
    string? Description,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Represents a parameter for document templates
/// </summary>
public record DocumentTemplateParameter(
    Guid Id,
    Guid? DocumentTemplateId,
    string? ParameterName,
    string? ParameterValue,
    string? ParameterType,
    bool IsActive,
    string StartDate,
    string EndDate
);
