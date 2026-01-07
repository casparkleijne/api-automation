namespace ApiAutomation.Api.Data.Entities;

/// <summary>
/// Base entity with common properties for all data entities
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
}

/// <summary>
/// Entity with code field
/// </summary>
public abstract class CodeEntity : BaseEntity
{
    public string? Code { get; set; }
}

/// <summary>
/// Entity with date range validity
/// </summary>
public abstract class ValidityEntity : CodeEntity
{
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.MaxValue;
}
