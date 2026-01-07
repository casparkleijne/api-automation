namespace ApiAutomation.Api.Repositories;

/// <summary>
/// Base interface for all entities with an Id
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}

/// <summary>
/// Interface for entities that have a code field
/// </summary>
public interface ICodeEntity : IEntity
{
    string? Code { get; }
}

/// <summary>
/// Interface for entities that can be activated/deactivated
/// </summary>
public interface IActivatable
{
    bool IsActive { get; }
}
