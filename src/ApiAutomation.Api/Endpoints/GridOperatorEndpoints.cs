using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class GridOperatorEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<GridOperator> _gridOperators = new()
    {
        new GridOperator(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Code: "LIANDER",
            Name: "Liander N.V.",
            Description: "Grid operator for Noord-Holland, Gelderland, and parts of other provinces",
            EanPrefix: "8712",
            Email: "info@liander.nl",
            Phone: "+31 88 542 6337",
            Website: "https://www.liander.nl",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new GridOperator(
            Id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Code: "STEDIN",
            Name: "Stedin Netbeheer B.V.",
            Description: "Grid operator for Zuid-Holland, Utrecht, and Zeeland",
            EanPrefix: "8713",
            Email: "info@stedin.net",
            Phone: "+31 88 896 3963",
            Website: "https://www.stedin.net",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new GridOperator(
            Id: Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Code: "ENEXIS",
            Name: "Enexis Netbeheer B.V.",
            Description: "Grid operator for Noord-Brabant, Limburg, and parts of other provinces",
            EanPrefix: "8714",
            Email: "info@enexis.nl",
            Phone: "+31 88 857 7777",
            Website: "https://www.enexis.nl",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapGridOperatorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/grid-operators")
            .WithTags("Grid Operators");

        // GET /api/v1/grid-operators - List all grid operators
        group.MapGet("/", (int? page, int? pageSize, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = activeOnly == true
                ? _gridOperators.Where(x => x.IsActive).ToList()
                : _gridOperators;

            var totalCount = filtered.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = filtered
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<GridOperator>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetGridOperators")
        .WithOpenApi(op =>
        {
            op.Summary = "List grid operators";
            op.Description = "Returns a paginated list of all grid operators (Netbeheerders). Supports filtering by active status.";
            return op;
        })
        .Produces<PaginatedResponse<GridOperator>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/grid-operators/{id} - Get single grid operator
        group.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _gridOperators.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Grid operator with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetGridOperator")
        .WithOpenApi(op =>
        {
            op.Summary = "Get grid operator by ID";
            op.Description = "Returns a single grid operator by its unique identifier.";
            return op;
        })
        .Produces<GridOperator>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/grid-operators/by-code/{code} - Get by code
        group.MapGet("/by-code/{code}", (string code) =>
        {
            var item = _gridOperators.FirstOrDefault(x =>
                x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Grid operator with code '{code}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetGridOperatorByCode")
        .WithOpenApi(op =>
        {
            op.Summary = "Get grid operator by code";
            op.Description = "Returns a single grid operator by its code (e.g., LIANDER, STEDIN, ENEXIS).";
            return op;
        })
        .Produces<GridOperator>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/grid-operators - Create new grid operator
        group.MapPost("/", (CreateGridOperatorRequest request) =>
        {
            // Check for duplicate code
            if (_gridOperators.Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Problem(
                    title: "Conflict",
                    detail: $"A grid operator with code '{request.Code}' already exists.",
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new GridOperator(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                EanPrefix: request.EanPrefix,
                Email: request.Email,
                Phone: request.Phone,
                Website: request.Website,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _gridOperators.Add(newItem);
            return Results.Created($"/api/v1/grid-operators/{newItem.Id}", newItem);
        })
        .WithName("CreateGridOperator")
        .WithOpenApi(op =>
        {
            op.Summary = "Create grid operator";
            op.Description = "Creates a new grid operator. The code must be unique.";
            return op;
        })
        .RequireAuthorization()
        .Produces<GridOperator>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/grid-operators/{id} - Update grid operator
        group.MapPut("/{id:guid}", (Guid id, UpdateGridOperatorRequest request) =>
        {
            var index = _gridOperators.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Grid operator with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _gridOperators[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                EanPrefix = request.EanPrefix ?? existing.EanPrefix,
                Email = request.Email ?? existing.Email,
                Phone = request.Phone ?? existing.Phone,
                Website = request.Website ?? existing.Website,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _gridOperators[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateGridOperator")
        .WithOpenApi(op =>
        {
            op.Summary = "Update grid operator";
            op.Description = "Updates an existing grid operator. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<GridOperator>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/grid-operators/{id} - Delete grid operator
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _gridOperators.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Grid operator with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _gridOperators.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteGridOperator")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete grid operator";
            op.Description = "Deletes an existing grid operator by its ID.";
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
