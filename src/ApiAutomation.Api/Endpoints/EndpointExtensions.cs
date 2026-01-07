using ApiAutomation.Api.Models;
using ApiAutomation.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

/// <summary>
/// Generic endpoint extensions for common CRUD operations (DRY principle)
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Maps standard CRUD endpoints for an entity
    /// </summary>
    public static RouteGroupBuilder MapCrudEndpoints<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapGetAll<TEntity, TCreate, TUpdate, TRepo>(entityName);
        group.MapGetById<TEntity, TCreate, TUpdate, TRepo>(entityName);
        group.MapCreate<TEntity, TCreate, TUpdate, TRepo>(entityName);
        group.MapUpdate<TEntity, TCreate, TUpdate, TRepo>(entityName);
        group.MapDelete<TEntity, TCreate, TUpdate, TRepo>(entityName);

        return group;
    }

    /// <summary>
    /// Maps GET all endpoint with pagination
    /// </summary>
    public static RouteGroupBuilder MapGetAll<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapGet("/", async (TRepo repo, int? page, int? pageSize) =>
        {
            var (currentPage, currentPageSize) = NormalizePagination(page, pageSize);
            var all = await repo.GetAllAsync();
            return PaginateResults(all, currentPage, currentPageSize);
        })
        .WithName($"Get{entityName}s")
        .WithOpenApi(op =>
        {
            op.Summary = $"List {entityName.ToLower()}s";
            op.Description = $"Returns a paginated list of all {entityName.ToLower()}s.";
            return op;
        })
        .Produces<PaginatedResponse<TEntity>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }

    /// <summary>
    /// Maps GET by ID endpoint
    /// </summary>
    public static RouteGroupBuilder MapGetById<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapGet("/{id:guid}", async (TRepo repo, Guid id) =>
        {
            var item = await repo.GetByIdAsync(id);
            return item is not null
                ? Results.Ok(item)
                : NotFound(entityName, id);
        })
        .WithName($"Get{entityName}")
        .WithOpenApi(op =>
        {
            op.Summary = $"Get {entityName.ToLower()} by ID";
            op.Description = $"Returns a single {entityName.ToLower()} by its unique identifier.";
            return op;
        })
        .Produces<TEntity>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }

    /// <summary>
    /// Maps GET by code endpoint for entities with a code field
    /// </summary>
    public static RouteGroupBuilder MapGetByCode<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity, ICodeEntity
        where TRepo : class, ICodeRepository<TEntity, TCreate, TUpdate>
    {
        group.MapGet("/by-code/{code}", async (TRepo repo, string code) =>
        {
            var item = await repo.GetByCodeAsync(code);
            return item is not null
                ? Results.Ok(item)
                : NotFoundByCode(entityName, code);
        })
        .WithName($"Get{entityName}ByCode")
        .WithOpenApi(op =>
        {
            op.Summary = $"Get {entityName.ToLower()} by code";
            op.Description = $"Returns a single {entityName.ToLower()} by its code.";
            return op;
        })
        .Produces<TEntity>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }

    /// <summary>
    /// Maps POST endpoint for creating entities
    /// </summary>
    public static RouteGroupBuilder MapCreate<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapPost("/", async (TRepo repo, TCreate request) =>
        {
            var item = await repo.CreateAsync(request);
            return Results.Created($"/{item.Id}", item);
        })
        .WithName($"Create{entityName}")
        .WithOpenApi(op =>
        {
            op.Summary = $"Create {entityName.ToLower()}";
            op.Description = $"Creates a new {entityName.ToLower()}.";
            return op;
        })
        .RequireAuthorization()
        .Produces<TEntity>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        return group;
    }

    /// <summary>
    /// Maps PUT endpoint for updating entities
    /// </summary>
    public static RouteGroupBuilder MapUpdate<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapPut("/{id:guid}", async (TRepo repo, Guid id, TUpdate request) =>
        {
            var item = await repo.UpdateAsync(id, request);
            return item is not null
                ? Results.Ok(item)
                : NotFound(entityName, id);
        })
        .WithName($"Update{entityName}")
        .WithOpenApi(op =>
        {
            op.Summary = $"Update {entityName.ToLower()}";
            op.Description = $"Updates an existing {entityName.ToLower()}.";
            return op;
        })
        .RequireAuthorization()
        .Produces<TEntity>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }

    /// <summary>
    /// Maps DELETE endpoint
    /// </summary>
    public static RouteGroupBuilder MapDelete<TEntity, TCreate, TUpdate, TRepo>(
        this RouteGroupBuilder group,
        string entityName)
        where TEntity : class, IEntity
        where TRepo : class, IRepository<TEntity, TCreate, TUpdate>
    {
        group.MapDelete("/{id:guid}", async (TRepo repo, Guid id) =>
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted
                ? Results.NoContent()
                : NotFound(entityName, id);
        })
        .WithName($"Delete{entityName}")
        .WithOpenApi(op =>
        {
            op.Summary = $"Delete {entityName.ToLower()}";
            op.Description = $"Deletes an existing {entityName.ToLower()}.";
            return op;
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }

    // Helper methods (Single Responsibility)

    private static (int page, int pageSize) NormalizePagination(int? page, int? pageSize) =>
        (Math.Max(1, page ?? 1), Math.Clamp(pageSize ?? 20, 1, 100));

    private static IResult PaginateResults<T>(IEnumerable<T> items, int page, int pageSize)
    {
        var list = items.ToList();
        var totalCount = list.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var pagedItems = list
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Results.Ok(new PaginatedResponse<T>(
            Items: pagedItems,
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        ));
    }

    private static IResult NotFound(string entityName, Guid id) =>
        Results.Problem(
            title: "Not Found",
            detail: $"{entityName} with ID '{id}' was not found.",
            statusCode: StatusCodes.Status404NotFound
        );

    private static IResult NotFoundByCode(string entityName, string code) =>
        Results.Problem(
            title: "Not Found",
            detail: $"{entityName} with code '{code}' was not found.",
            statusCode: StatusCodes.Status404NotFound
        );
}
