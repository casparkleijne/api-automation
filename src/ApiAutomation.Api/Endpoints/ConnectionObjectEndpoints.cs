using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class ConnectionObjectEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<ConnectionObject> _connectionObjects = new()
    {
        new ConnectionObject(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Ean: "871234567890123456",
            PostalCode: "1234AB",
            HouseNumber: 1,
            HouseNumberAddition: null,
            Street: "Main Street",
            City: "Amsterdam",
            Country: "NL",
            Status: "Active",
            GridOperatorId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapConnectionObjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/connection-objects")
            .WithTags("Connection Objects");

        // GET /api/v1/connection-objects - List all connection objects
        group.MapGet("/", (int? page, int? pageSize) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);
            var totalCount = _connectionObjects.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = _connectionObjects
                .Skip((currentPage - 1) * currentPageSize)
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
            op.Description = "Returns a paginated list of all connection objects. Supports pagination via page and pageSize query parameters.";
            return op;
        })
        .Produces<PaginatedResponse<ConnectionObject>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

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

        // GET /api/v1/connection-objects/by-ean/{ean} - Get by EAN code
        group.MapGet("/by-ean/{ean}", (string ean) =>
        {
            var item = _connectionObjects.FirstOrDefault(x => x.Ean == ean);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Connection object with EAN '{ean}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetConnectionObjectByEan")
        .WithOpenApi(op =>
        {
            op.Summary = "Get connection object by EAN";
            op.Description = "Returns a single connection object by its EAN code.";
            return op;
        })
        .Produces<ConnectionObject>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/connection-objects - Create new connection object
        group.MapPost("/", (CreateConnectionObjectRequest request) =>
        {
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new ConnectionObject(
                Id: Guid.NewGuid(),
                Ean: request.Ean,
                PostalCode: request.PostalCode,
                HouseNumber: request.HouseNumber,
                HouseNumberAddition: request.HouseNumberAddition,
                Street: request.Street,
                City: request.City,
                Country: request.Country ?? "NL",
                Status: "Pending",
                GridOperatorId: request.GridOperatorId,
                CreatedAt: now,
                ModifiedAt: null
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
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Ean = request.Ean ?? existing.Ean,
                PostalCode = request.PostalCode ?? existing.PostalCode,
                HouseNumber = request.HouseNumber ?? existing.HouseNumber,
                HouseNumberAddition = request.HouseNumberAddition ?? existing.HouseNumberAddition,
                Street = request.Street ?? existing.Street,
                City = request.City ?? existing.City,
                Country = request.Country ?? existing.Country,
                Status = request.Status ?? existing.Status,
                GridOperatorId = request.GridOperatorId ?? existing.GridOperatorId,
                ModifiedAt = now
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
    }
}
