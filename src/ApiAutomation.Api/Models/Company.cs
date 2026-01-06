namespace ApiAutomation.Api.Models;

/// <summary>
/// Represents a company
/// </summary>
public record Company(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Company name</summary>
    string? Name,
    /// <summary>Chamber of Commerce number (KvK)</summary>
    string? KvkNumber,
    /// <summary>VAT number (BTW)</summary>
    string? VatNumber,
    /// <summary>IBAN bank account number</summary>
    string? Iban,
    /// <summary>Whether VAT is shifted (BTW verlegd)</summary>
    bool VatShifted,
    /// <summary>Primary address ID</summary>
    int? PrimaryAddressId,
    /// <summary>Secondary address ID</summary>
    int? SecondaryAddressId,
    /// <summary>Primary post box address ID</summary>
    int? PrimaryPostBoxAddressId,
    /// <summary>Secondary post box address ID</summary>
    int? SecondaryPostBoxAddressId,
    /// <summary>Whether the company is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a company
/// </summary>
public record CreateCompanyRequest(
    string? Name,
    string? KvkNumber,
    string? VatNumber,
    string? Iban,
    bool VatShifted,
    int? PrimaryAddressId,
    int? SecondaryAddressId,
    int? PrimaryPostBoxAddressId,
    int? SecondaryPostBoxAddressId,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a company
/// </summary>
public record UpdateCompanyRequest(
    string? Name,
    string? KvkNumber,
    string? VatNumber,
    string? Iban,
    bool? VatShifted,
    int? PrimaryAddressId,
    int? SecondaryAddressId,
    int? PrimaryPostBoxAddressId,
    int? SecondaryPostBoxAddressId,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);

/// <summary>
/// Represents a contact person
/// </summary>
public record ContactPerson(
    /// <summary>Unique identifier</summary>
    Guid Id,
    /// <summary>Gender (M/F/X)</summary>
    string? Gender,
    /// <summary>Initials</summary>
    string? Initials,
    /// <summary>First name</summary>
    string? FirstName,
    /// <summary>Middle name / prefix</summary>
    string? MiddleName,
    /// <summary>Last name</summary>
    string? LastName,
    /// <summary>Primary email address</summary>
    string? PrimaryEmail,
    /// <summary>Secondary email address</summary>
    string? SecondaryEmail,
    /// <summary>Primary phone number</summary>
    string? PrimaryPhone,
    /// <summary>Secondary phone number</summary>
    string? SecondaryPhone,
    /// <summary>Fax number</summary>
    string? Fax,
    /// <summary>Whether the contact person is active</summary>
    bool IsActive,
    /// <summary>Start date in ISO 8601 format</summary>
    string StartDate,
    /// <summary>End date in ISO 8601 format</summary>
    string EndDate
);

/// <summary>
/// Request model for creating a contact person
/// </summary>
public record CreateContactPersonRequest(
    string? Gender,
    string? Initials,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? PrimaryEmail,
    string? SecondaryEmail,
    string? PrimaryPhone,
    string? SecondaryPhone,
    string? Fax,
    string StartDate,
    string EndDate
);

/// <summary>
/// Request model for updating a contact person
/// </summary>
public record UpdateContactPersonRequest(
    string? Gender,
    string? Initials,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? PrimaryEmail,
    string? SecondaryEmail,
    string? PrimaryPhone,
    string? SecondaryPhone,
    string? Fax,
    bool? IsActive,
    string? StartDate,
    string? EndDate
);
