namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a grid operator (Netbeheerder) responsible for managing energy infrastructure
/// </summary>
public record GridOperator(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Grid operator name</summary>
    string? Name,
    /// <summary>Grid operator code</summary>
    string? GridOperatorCode,
    /// <summary>Processor code for integration</summary>
    string? ProcessorCode,
    /// <summary>Website URL</summary>
    string? WebsiteUri,
    /// <summary>Department name</summary>
    string? Department,
    /// <summary>Primary email address</summary>
    string? PrimaryEmailAddress,
    /// <summary>Contact person ID</summary>
    int? ContactPersonId,
    /// <summary>Company ID</summary>
    Guid? CompanyId,
    /// <summary>Primary address ID</summary>
    int? PrimaryAddressId,
    /// <summary>Secondary address ID</summary>
    int? SecondaryAddressId,
    /// <summary>Primary post box address ID</summary>
    int? PrimaryPostBoxAddressId,
    /// <summary>Secondary post box address ID</summary>
    int? SecondaryPostBoxAddressId,
    /// <summary>Customer service ID</summary>
    Guid? CustomerServiceId,
    /// <summary>Customer service details</summary>
    CustomerService? CustomerService,
    /// <summary>Whether connection ready date is active</summary>
    bool AansluitGereedDatumActive,
    /// <summary>Whether the grid operator is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a grid operator
/// </summary>
public record CreateGridOperatorRequest(
    string Name,
    string? GridOperatorCode,
    string? ProcessorCode,
    string? WebsiteUri,
    string? Department,
    string? PrimaryEmailAddress,
    int? ContactPersonId,
    Guid? CompanyId,
    int? PrimaryAddressId,
    int? SecondaryAddressId,
    int? PrimaryPostBoxAddressId,
    int? SecondaryPostBoxAddressId,
    Guid? CustomerServiceId,
    bool AansluitGereedDatumActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a grid operator
/// </summary>
public record UpdateGridOperatorRequest(
    string? Name,
    string? GridOperatorCode,
    string? ProcessorCode,
    string? WebsiteUri,
    string? Department,
    string? PrimaryEmailAddress,
    int? ContactPersonId,
    Guid? CompanyId,
    int? PrimaryAddressId,
    int? SecondaryAddressId,
    int? PrimaryPostBoxAddressId,
    int? SecondaryPostBoxAddressId,
    Guid? CustomerServiceId,
    bool? AansluitGereedDatumActive,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Customer service information for a grid operator
/// </summary>
public record CustomerService(
    Guid Id,
    string? PrimaryWebsiteUri,
    string? TariffWebsiteUri,
    string? Info,
    string? FirstPhone,
    string? SecondPhone,
    string? ThirdPhone,
    string? FourthPhone,
    bool IsActive,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for creating customer service
/// </summary>
public record CreateCustomerServiceRequest(
    string? PrimaryWebsiteUri,
    string? TariffWebsiteUri,
    string? Info,
    string? FirstPhone,
    string? SecondPhone,
    string? ThirdPhone,
    string? FourthPhone,
    string StartDate,
    string EndDate
);
