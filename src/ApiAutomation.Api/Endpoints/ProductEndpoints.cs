using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class ProductEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<Product> _products = new()
    {
        new Product(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888881"),
            Code: "EL-CONN-STD",
            Name: "Standard Electricity Connection",
            Description: "Standard residential electricity connection up to 3x25A",
            DisciplineId: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            SubServiceId: Guid.Parse("77777777-7777-7777-7777-777777777771"),
            GridOperatorId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ProductType: "Connection",
            IsActive: true,
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Product(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888882"),
            Code: "EL-CONN-HVY",
            Name: "Heavy Electricity Connection",
            Description: "Heavy duty electricity connection above 3x25A",
            DisciplineId: Guid.Parse("55555555-5555-5555-5555-555555555551"),
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            SubServiceId: Guid.Parse("77777777-7777-7777-7777-777777777772"),
            GridOperatorId: null,
            ProductType: "Connection",
            IsActive: true,
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Product(
            Id: Guid.Parse("88888888-8888-8888-8888-888888888883"),
            Code: "GAS-CONN-STD",
            Name: "Standard Gas Connection",
            Description: "Standard residential gas connection",
            DisciplineId: Guid.Parse("55555555-5555-5555-5555-555555555552"),
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            SubServiceId: null,
            GridOperatorId: null,
            ProductType: "Connection",
            IsActive: true,
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/products")
            .WithTags("Products");

        // GET /api/v1/products - List all products
        group.MapGet("/", (int? page, int? pageSize, Guid? disciplineId, Guid? serviceId,
            Guid? subServiceId, Guid? gridOperatorId, string? productType, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _products.AsEnumerable();
            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (disciplineId.HasValue) filtered = filtered.Where(x => x.DisciplineId == disciplineId);
            if (serviceId.HasValue) filtered = filtered.Where(x => x.ServiceId == serviceId);
            if (subServiceId.HasValue) filtered = filtered.Where(x => x.SubServiceId == subServiceId);
            if (gridOperatorId.HasValue) filtered = filtered.Where(x => x.GridOperatorId == gridOperatorId);
            if (!string.IsNullOrEmpty(productType)) filtered = filtered.Where(x =>
                x.ProductType?.Equals(productType, StringComparison.OrdinalIgnoreCase) == true);

            var list = filtered.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<Product>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetProducts")
        .WithOpenApi(op =>
        {
            op.Summary = "List products";
            op.Description = "Returns a paginated list of all products. Supports filtering by discipline, service, sub-service, grid operator, and product type.";
            return op;
        })
        .Produces<PaginatedResponse<Product>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/products/{id} - Get single product
        group.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _products.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Product with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetProduct")
        .WithOpenApi(op =>
        {
            op.Summary = "Get product by ID";
            op.Description = "Returns a single product by its unique identifier.";
            return op;
        })
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/products/by-code/{code} - Get by code
        group.MapGet("/by-code/{code}", (string code) =>
        {
            var item = _products.FirstOrDefault(x =>
                x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Product with code '{code}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetProductByCode")
        .WithOpenApi(op =>
        {
            op.Summary = "Get product by code";
            op.Description = "Returns a single product by its code.";
            return op;
        })
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/products - Create new product
        group.MapPost("/", (CreateProductRequest request) =>
        {
            if (_products.Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Problem(
                    title: "Conflict",
                    detail: $"A product with code '{request.Code}' already exists.",
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new Product(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                DisciplineId: request.DisciplineId,
                ServiceId: request.ServiceId,
                SubServiceId: request.SubServiceId,
                GridOperatorId: request.GridOperatorId,
                ProductType: request.ProductType,
                IsActive: true,
                EffectiveFrom: request.EffectiveFrom,
                EffectiveTo: request.EffectiveTo,
                CreatedAt: now,
                ModifiedAt: null
            );

            _products.Add(newItem);
            return Results.Created($"/api/v1/products/{newItem.Id}", newItem);
        })
        .WithName("CreateProduct")
        .WithOpenApi(op =>
        {
            op.Summary = "Create product";
            op.Description = "Creates a new product. The code must be unique.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Product>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/products/{id} - Update product
        group.MapPut("/{id:guid}", (Guid id, UpdateProductRequest request) =>
        {
            var index = _products.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Product with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _products[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                DisciplineId = request.DisciplineId ?? existing.DisciplineId,
                ServiceId = request.ServiceId ?? existing.ServiceId,
                SubServiceId = request.SubServiceId ?? existing.SubServiceId,
                GridOperatorId = request.GridOperatorId ?? existing.GridOperatorId,
                ProductType = request.ProductType ?? existing.ProductType,
                IsActive = request.IsActive ?? existing.IsActive,
                EffectiveFrom = request.EffectiveFrom ?? existing.EffectiveFrom,
                EffectiveTo = request.EffectiveTo ?? existing.EffectiveTo,
                ModifiedAt = now
            };

            _products[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdateProduct")
        .WithOpenApi(op =>
        {
            op.Summary = "Update product";
            op.Description = "Updates an existing product. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/products/{id} - Delete product
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _products.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Product with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _products.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete product";
            op.Description = "Deletes an existing product by its ID.";
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
