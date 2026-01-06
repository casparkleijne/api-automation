using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class ServiceEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<Service> _services = new()
    {
        new Service(
            Id: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            Code: "NEW_CONN",
            Name: "New Connection",
            Description: "Request a new connection to the energy grid",
            DisciplineId: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Service(
            Id: Guid.Parse("66666666-6666-6666-6666-666666666662"),
            Code: "MODIFY_CONN",
            Name: "Modify Connection",
            Description: "Modify an existing connection",
            DisciplineId: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Service(
            Id: Guid.Parse("66666666-6666-6666-6666-666666666663"),
            Code: "DISCONNECT",
            Name: "Disconnect",
            Description: "Request disconnection from the energy grid",
            DisciplineId: null,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    private static readonly List<SubService> _subServices = new()
    {
        new SubService(
            Id: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            Code: "STANDARD",
            Name: "Standard Connection",
            Description: "Standard residential connection up to 3x25A",
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new SubService(
            Id: Guid.Parse("77777777-7777-7777-7777-777777777772"),
            Code: "HEAVY",
            Name: "Heavy Connection",
            Description: "Heavy duty connection above 3x25A",
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new SubService(
            Id: Guid.Parse("77777777-7777-7777-7777-777777777773"),
            Code: "CAPACITY_UPGRADE",
            Name: "Capacity Upgrade",
            Description: "Upgrade connection capacity",
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666662"),
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapServiceEndpoints(this WebApplication app)
    {
        var serviceGroup = app.MapGroup("/api/v1/services")
            .WithTags("Services");

        // GET /api/v1/services - List all services
        serviceGroup.MapGet("/", (int? page, int? pageSize, Guid? disciplineId, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _services.AsEnumerable();
            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (disciplineId.HasValue) filtered = filtered.Where(x => x.DisciplineId == disciplineId);

            var list = filtered.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<Service>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetServices")
        .WithOpenApi(op =>
        {
            op.Summary = "List services";
            op.Description = "Returns a paginated list of all services. Can be filtered by discipline.";
            return op;
        })
        .Produces<PaginatedResponse<Service>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/services/{id} - Get single service
        serviceGroup.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _services.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetService")
        .WithOpenApi(op =>
        {
            op.Summary = "Get service by ID";
            op.Description = "Returns a single service by its unique identifier.";
            return op;
        })
        .Produces<Service>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/services/{id}/sub-services - Get sub-services for a service
        serviceGroup.MapGet("/{id:guid}/sub-services", (Guid id, int? page, int? pageSize) =>
        {
            if (!_services.Any(x => x.Id == id))
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _subServices.Where(x => x.ServiceId == id).ToList();
            var totalCount = filtered.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = filtered
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<SubService>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetServiceSubServices")
        .WithOpenApi(op =>
        {
            op.Summary = "List sub-services for a service";
            op.Description = "Returns all sub-services belonging to a specific service.";
            return op;
        })
        .Produces<PaginatedResponse<SubService>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/services - Create new service
        serviceGroup.MapPost("/", (CreateServiceRequest request) =>
        {
            if (_services.Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Problem(
                    title: "Conflict",
                    detail: $"A service with code '{request.Code}' already exists.",
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new Service(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                DisciplineId: request.DisciplineId,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _services.Add(newItem);
            return Results.Created($"/api/v1/services/{newItem.Id}", newItem);
        })
        .WithName("CreateService")
        .WithOpenApi(op =>
        {
            op.Summary = "Create service";
            op.Description = "Creates a new service. The code must be unique.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Service>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/services/{id} - Update service
        serviceGroup.MapPut("/{id:guid}", (Guid id, UpdateServiceRequest request) =>
        {
            var index = _services.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _services[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                DisciplineId = request.DisciplineId ?? existing.DisciplineId,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _services[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateService")
        .WithOpenApi(op =>
        {
            op.Summary = "Update service";
            op.Description = "Updates an existing service. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Service>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/services/{id} - Delete service
        serviceGroup.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _services.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _services.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteService")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete service";
            op.Description = "Deletes an existing service by its ID.";
            return op;
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // Sub-Services endpoints
        var subServiceGroup = app.MapGroup("/api/v1/sub-services")
            .WithTags("Sub-Services");

        // GET /api/v1/sub-services - List all sub-services
        subServiceGroup.MapGet("/", (int? page, int? pageSize, Guid? serviceId, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _subServices.AsEnumerable();
            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (serviceId.HasValue) filtered = filtered.Where(x => x.ServiceId == serviceId);

            var list = filtered.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<SubService>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetSubServices")
        .WithOpenApi(op =>
        {
            op.Summary = "List sub-services";
            op.Description = "Returns a paginated list of all sub-services. Can be filtered by service.";
            return op;
        })
        .Produces<PaginatedResponse<SubService>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/sub-services/{id} - Get single sub-service
        subServiceGroup.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _subServices.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Sub-service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetSubService")
        .WithOpenApi(op =>
        {
            op.Summary = "Get sub-service by ID";
            op.Description = "Returns a single sub-service by its unique identifier.";
            return op;
        })
        .Produces<SubService>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/sub-services - Create new sub-service
        subServiceGroup.MapPost("/", (CreateSubServiceRequest request) =>
        {
            if (!_services.Any(x => x.Id == request.ServiceId))
            {
                return Results.Problem(
                    title: "Bad Request",
                    detail: $"Service with ID '{request.ServiceId}' was not found.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new SubService(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                ServiceId: request.ServiceId,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _subServices.Add(newItem);
            return Results.Created($"/api/v1/sub-services/{newItem.Id}", newItem);
        })
        .WithName("CreateSubService")
        .WithOpenApi(op =>
        {
            op.Summary = "Create sub-service";
            op.Description = "Creates a new sub-service for a specific service.";
            return op;
        })
        .RequireAuthorization()
        .Produces<SubService>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/sub-services/{id} - Update sub-service
        subServiceGroup.MapPut("/{id:guid}", (Guid id, UpdateSubServiceRequest request) =>
        {
            var index = _subServices.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Sub-service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _subServices[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _subServices[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateSubService")
        .WithOpenApi(op =>
        {
            op.Summary = "Update sub-service";
            op.Description = "Updates an existing sub-service. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<SubService>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/sub-services/{id} - Delete sub-service
        subServiceGroup.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _subServices.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Sub-service with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _subServices.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteSubService")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete sub-service";
            op.Description = "Deletes an existing sub-service by its ID.";
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
