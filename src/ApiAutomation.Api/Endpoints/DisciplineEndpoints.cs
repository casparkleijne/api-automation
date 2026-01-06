using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class DisciplineEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<Discipline> _disciplines = new()
    {
        new Discipline(
            Id: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            Code: "EL",
            Name: "Electricity",
            Description: "Electrical connections and services",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Discipline(
            Id: Guid.Parse("55555555-5555-5555-5555-555555555552"),
            Code: "GAS",
            Name: "Gas",
            Description: "Gas connections and services",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Discipline(
            Id: Guid.Parse("55555555-5555-5555-5555-555555555553"),
            Code: "HEAT",
            Name: "District Heating",
            Description: "District heating connections and services",
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapDisciplineEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/disciplines")
            .WithTags("Disciplines");

        // GET /api/v1/disciplines - List all disciplines
        group.MapGet("/", (int? page, int? pageSize, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = activeOnly == true
                ? _disciplines.Where(x => x.IsActive).ToList()
                : _disciplines;

            var totalCount = filtered.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = filtered
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<Discipline>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetDisciplines")
        .WithOpenApi(op =>
        {
            op.Summary = "List disciplines";
            op.Description = "Returns a paginated list of all disciplines (e.g., Electricity, Gas, District Heating).";
            return op;
        })
        .Produces<PaginatedResponse<Discipline>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/disciplines/{id} - Get single discipline
        group.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _disciplines.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Discipline with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetDiscipline")
        .WithOpenApi(op =>
        {
            op.Summary = "Get discipline by ID";
            op.Description = "Returns a single discipline by its unique identifier.";
            return op;
        })
        .Produces<Discipline>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/disciplines/by-code/{code} - Get by code
        group.MapGet("/by-code/{code}", (string code) =>
        {
            var item = _disciplines.FirstOrDefault(x =>
                x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Discipline with code '{code}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetDisciplineByCode")
        .WithOpenApi(op =>
        {
            op.Summary = "Get discipline by code";
            op.Description = "Returns a single discipline by its code (e.g., EL, GAS, HEAT).";
            return op;
        })
        .Produces<Discipline>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/disciplines - Create new discipline
        group.MapPost("/", (CreateDisciplineRequest request) =>
        {
            if (_disciplines.Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Problem(
                    title: "Conflict",
                    detail: $"A discipline with code '{request.Code}' already exists.",
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new Discipline(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _disciplines.Add(newItem);
            return Results.Created($"/api/v1/disciplines/{newItem.Id}", newItem);
        })
        .WithName("CreateDiscipline")
        .WithOpenApi(op =>
        {
            op.Summary = "Create discipline";
            op.Description = "Creates a new discipline. The code must be unique.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Discipline>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/disciplines/{id} - Update discipline
        group.MapPut("/{id:guid}", (Guid id, UpdateDisciplineRequest request) =>
        {
            var index = _disciplines.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Discipline with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _disciplines[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _disciplines[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateDiscipline")
        .WithOpenApi(op =>
        {
            op.Summary = "Update discipline";
            op.Description = "Updates an existing discipline. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Discipline>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/disciplines/{id} - Delete discipline
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _disciplines.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Discipline with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _disciplines.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteDiscipline")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete discipline";
            op.Description = "Deletes an existing discipline by its ID.";
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
