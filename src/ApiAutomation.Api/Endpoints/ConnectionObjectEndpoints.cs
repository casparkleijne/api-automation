using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class ConnectionObjectEndpoints
{
    private static readonly string _now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    private static readonly string _future = DateTime.UtcNow.AddYears(10).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

    // In-memory storage for demo purposes
    private static readonly List<ConnectionObject> _connectionObjects = new()
    {
        new ConnectionObject(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name: "Residential Connection",
            Description: "Standard residential electricity connection",
            Code: "RES-001",
            Priority: 1,
            ProfileType: 1,
            AddressType: 1,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2035-01-01T00:00:00.000Z"
        )
    };

    public static void MapConnectionObjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/connection-objects")
            .WithTags("Connection Objects");

        // GET /api/v1/connection-objects - List all connection objects
        group.MapGet("/", (int? page, int? pageSize, string? filter, int? skip, int? take) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? take ?? 20, 1, 100);
            var skipCount = skip ?? (currentPage - 1) * currentPageSize;

            var filtered = _connectionObjects.AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                filtered = filtered.Where(x =>
                    (x.Name?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Code?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            var list = filtered.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip(skipCount)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<ConnectionObject>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetConnectionObjects")
        .WithOpenApi(op =>
        {
            op.Summary = "List connection objects";
            op.Description = "Returns a paginated list of all connection objects. Supports filtering and pagination.";
            return op;
        })
        .Produces<PaginatedResponse<ConnectionObject>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/connection-objects/count - Count connection objects
        group.MapGet("/count", (string? filter) =>
        {
            var filtered = _connectionObjects.AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                filtered = filtered.Where(x =>
                    (x.Name?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Code?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false));
            }
            return Results.Ok(new { count = filtered.Count() });
        })
        .WithName("CountConnectionObjects")
        .WithOpenApi(op =>
        {
            op.Summary = "Count connection objects";
            op.Description = "Returns the count of connection objects matching the filter.";
            return op;
        })
        .Produces<object>(StatusCodes.Status200OK);

        // GET /api/v1/connection-objects/{id} - Get single connection object
        group.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _connectionObjects.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetConnectionObject")
        .WithOpenApi(op =>
        {
            op.Summary = "Get connection object by ID";
            op.Description = "Returns a single connection object by its unique identifier.";
            return op;
        })
        .Produces<ConnectionObject>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/connection-objects/by-code/{code} - Get by code
        group.MapGet("/by-code/{code}", (string code) =>
        {
            var item = _connectionObjects.FirstOrDefault(x =>
                x.Code?.Equals(code, StringComparison.OrdinalIgnoreCase) ?? false);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with code '{code}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetConnectionObjectByCode")
        .WithOpenApi(op =>
        {
            op.Summary = "Get connection object by code";
            op.Description = "Returns a single connection object by its code.";
            return op;
        })
        .Produces<ConnectionObject>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/connection-objects - Create new connection object
        group.MapPost("/", (CreateConnectionObjectRequest request) =>
        {
            var newItem = new ConnectionObject(
                Id: Guid.NewGuid(),
                Name: request.Name,
                Description: request.Description,
                Code: request.Code,
                Priority: request.Priority,
                ProfileType: request.ProfileType,
                AddressType: request.AddressType,
                IsActive: true,
                StartDate: request.StartDate,
                EndDate: request.EndDate
            );

            _connectionObjects.Add(newItem);
            return Results.Created($"/api/v1/connection-objects/{newItem.Id}", newItem);
        })
        .WithName("CreateConnectionObject")
        .WithOpenApi(op =>
        {
            op.Summary = "Create connection object";
            op.Description = "Creates a new connection object. Returns the created object with its generated ID.";
            return op;
        })
        .RequireAuthorization()
        .Produces<ConnectionObject>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/connection-objects/{id} - Update connection object
        group.MapPut("/{id:guid}", (Guid id, UpdateConnectionObjectRequest request) =>
        {
            var index = _connectionObjects.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _connectionObjects[index];
            var updated = existing with
            {
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                Code = request.Code ?? existing.Code,
                Priority = request.Priority ?? existing.Priority,
                ProfileType = request.ProfileType ?? existing.ProfileType,
                AddressType = request.AddressType ?? existing.AddressType,
                IsActive = request.IsActive ?? existing.IsActive,
                StartDate = request.StartDate ?? existing.StartDate,
                EndDate = request.EndDate ?? existing.EndDate
            };

            _connectionObjects[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateConnectionObject")
        .WithOpenApi(op =>
        {
            op.Summary = "Update connection object";
            op.Description = "Updates an existing connection object. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<ConnectionObject>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PATCH /api/v1/connection-objects/{id} - Partial update
        group.MapPatch("/{id:guid}", (Guid id, UpdateConnectionObjectRequest request) =>
        {
            var index = _connectionObjects.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _connectionObjects[index];
            var updated = existing with
            {
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                Code = request.Code ?? existing.Code,
                Priority = request.Priority ?? existing.Priority,
                ProfileType = request.ProfileType ?? existing.ProfileType,
                AddressType = request.AddressType ?? existing.AddressType,
                IsActive = request.IsActive ?? existing.IsActive,
                StartDate = request.StartDate ?? existing.StartDate,
                EndDate = request.EndDate ?? existing.EndDate
            };

            _connectionObjects[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("PatchConnectionObject")
        .WithOpenApi(op =>
        {
            op.Summary = "Patch connection object";
            op.Description = "Partially updates an existing connection object.";
            return op;
        })
        .RequireAuthorization()
        .Produces<ConnectionObject>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        // DELETE /api/v1/connection-objects/{id} - Delete connection object
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _connectionObjects.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _connectionObjects.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteConnectionObject")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete connection object";
            op.Description = "Deletes an existing connection object by its ID.";
            return op;
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/connection-objects/delete - Delete multiple
        group.MapPost("/delete", (List<Guid> ids) =>
        {
            var removedCount = 0;
            foreach (var id in ids)
            {
                var index = _connectionObjects.FindIndex(x => x.Id == id);
                if (index >= 0)
                {
                    _connectionObjects.RemoveAt(index);
                    removedCount++;
                }
            }
            return Results.Ok(new { deletedCount = removedCount });
        })
        .WithName("DeleteMultipleConnectionObjects")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete multiple connection objects";
            op.Description = "Deletes multiple connection objects by their IDs.";
            return op;
        })
        .RequireAuthorization()
        .Produces<object>(StatusCodes.Status200OK);
    }
}
