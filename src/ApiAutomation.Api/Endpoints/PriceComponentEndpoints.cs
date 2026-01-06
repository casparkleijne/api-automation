using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class PriceComponentEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<PriceComponent> _priceComponents = new()
    {
        new PriceComponent(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            Code: "CONN-FEE-STD",
            Name: "Standard Connection Fee",
            Description: "One-time fee for standard electricity connection",
            ProductId: Guid.Parse("88888888-8888-8888-8888-888888888881"),
            ServiceId: null,
            GridOperatorId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Amount: 595.00m,
            Currency: "EUR",
            Unit: "once",
            VatPercentage: 21.00m,
            VatIncluded: false,
            PriceType: "fixed",
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new PriceComponent(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
            Code: "CONN-FEE-HVY",
            Name: "Heavy Connection Fee",
            Description: "One-time fee for heavy duty electricity connection",
            ProductId: Guid.Parse("88888888-8888-8888-8888-888888888882"),
            ServiceId: null,
            GridOperatorId: null,
            Amount: 1250.00m,
            Currency: "EUR",
            Unit: "once",
            VatPercentage: 21.00m,
            VatIncluded: false,
            PriceType: "fixed",
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new PriceComponent(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
            Code: "METER-COST",
            Name: "Meter Installation Cost",
            Description: "Cost per meter distance from the main grid",
            ProductId: null,
            ServiceId: Guid.Parse("66666666-6666-6666-6666-666666666661"),
            GridOperatorId: null,
            Amount: 125.50m,
            Currency: "EUR",
            Unit: "meter",
            VatPercentage: 21.00m,
            VatIncluded: false,
            PriceType: "per-unit",
            EffectiveFrom: "2025-01-01T00:00:00.000Z",
            EffectiveTo: null,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapPriceComponentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/v1/price-components")
            .WithTags("Price Components");

        // GET /api/v1/price-components - List all price components
        group.MapGet("/", (int? page, int? pageSize, Guid? productId, Guid? serviceId,
            Guid? gridOperatorId, string? priceType, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _priceComponents.AsEnumerable();
            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (productId.HasValue) filtered = filtered.Where(x => x.ProductId == productId);
            if (serviceId.HasValue) filtered = filtered.Where(x => x.ServiceId == serviceId);
            if (gridOperatorId.HasValue) filtered = filtered.Where(x => x.GridOperatorId == gridOperatorId);
            if (!string.IsNullOrEmpty(priceType)) filtered = filtered.Where(x =>
                x.PriceType?.Equals(priceType, StringComparison.OrdinalIgnoreCase) == true);

            var list = filtered.ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<PriceComponent>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetPriceComponents")
        .WithOpenApi(op =>
        {
            op.Summary = "List price components";
            op.Description = "Returns a paginated list of all price components. Supports filtering by product, service, grid operator, and price type.";
            return op;
        })
        .Produces<PaginatedResponse<PriceComponent>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/price-components/{id} - Get single price component
        group.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _priceComponents.FirstOrDefault(x => x.Id == id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Price component with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetPriceComponent")
        .WithOpenApi(op =>
        {
            op.Summary = "Get price component by ID";
            op.Description = "Returns a single price component by its unique identifier.";
            return op;
        })
        .Produces<PriceComponent>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/price-components/by-code/{code} - Get by code
        group.MapGet("/by-code/{code}", (string code) =>
        {
            var item = _priceComponents.FirstOrDefault(x =>
                x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Price component with code '{code}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetPriceComponentByCode")
        .WithOpenApi(op =>
        {
            op.Summary = "Get price component by code";
            op.Description = "Returns a single price component by its code.";
            return op;
        })
        .Produces<PriceComponent>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/price-components - Create new price component
        group.MapPost("/", (CreatePriceComponentRequest request) =>
        {
            if (_priceComponents.Any(x => x.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.Problem(
                    title: "Conflict",
                    detail: $"A price component with code '{request.Code}' already exists.",
                    statusCode: StatusCodes.Status409Conflict
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new PriceComponent(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Name: request.Name,
                Description: request.Description,
                ProductId: request.ProductId,
                ServiceId: request.ServiceId,
                GridOperatorId: request.GridOperatorId,
                Amount: request.Amount,
                Currency: request.Currency,
                Unit: request.Unit,
                VatPercentage: request.VatPercentage,
                VatIncluded: request.VatIncluded,
                PriceType: request.PriceType,
                EffectiveFrom: request.EffectiveFrom,
                EffectiveTo: request.EffectiveTo,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _priceComponents.Add(newItem);
            return Results.Created($"/api/v1/price-components/{newItem.Id}", newItem);
        })
        .WithName("CreatePriceComponent")
        .WithOpenApi(op =>
        {
            op.Summary = "Create price component";
            op.Description = "Creates a new price component. The code must be unique.";
            return op;
        })
        .RequireAuthorization()
        .Produces<PriceComponent>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/price-components/{id} - Update price component
        group.MapPut("/{id:guid}", (Guid id, UpdatePriceComponentRequest request) =>
        {
            var index = _priceComponents.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Price component with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _priceComponents[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                ProductId = request.ProductId ?? existing.ProductId,
                ServiceId = request.ServiceId ?? existing.ServiceId,
                GridOperatorId = request.GridOperatorId ?? existing.GridOperatorId,
                Amount = request.Amount ?? existing.Amount,
                Currency = request.Currency ?? existing.Currency,
                Unit = request.Unit ?? existing.Unit,
                VatPercentage = request.VatPercentage ?? existing.VatPercentage,
                VatIncluded = request.VatIncluded ?? existing.VatIncluded,
                PriceType = request.PriceType ?? existing.PriceType,
                EffectiveFrom = request.EffectiveFrom ?? existing.EffectiveFrom,
                EffectiveTo = request.EffectiveTo ?? existing.EffectiveTo,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _priceComponents[index] = updated;
            return Results.Ok(updated);
        })
        .WithName("UpdatePriceComponent")
        .WithOpenApi(op =>
        {
            op.Summary = "Update price component";
            op.Description = "Updates an existing price component. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<PriceComponent>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/price-components/{id} - Delete price component
        group.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _priceComponents.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Price component with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _priceComponents.RemoveAt(index);
            return Results.NoContent();
        })
        .WithName("DeletePriceComponent")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete price component";
            op.Description = "Deletes an existing price component by its ID.";
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
