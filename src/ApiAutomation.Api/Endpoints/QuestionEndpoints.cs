using ApiAutomation.Api.Models;
using ApiAutomation.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class QuestionEndpoints
{
    public static void MapQuestionEndpoints(this WebApplication app)
    {
        var questionGroup = app.MapGroup("/api/v1/questions")
            .WithTags("Questions");

        // GET /api/v1/questions - List all questions
        questionGroup.MapGet("/", async (
            IQuestionRepository repo,
            int? page,
            int? pageSize,
            Guid? answerTypeId,
            bool? activeOnly,
            bool? mandatoryOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var all = await repo.GetAllAsync();
            var filtered = all.AsEnumerable();

            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (mandatoryOnly == true) filtered = filtered.Where(x => x.IsMandatory);
            if (answerTypeId.HasValue) filtered = filtered.Where(x => x.AnswerTypeId == answerTypeId);

            var list = filtered.OrderBy(x => x.Priority).ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<Question>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetQuestions")
        .WithOpenApi(op =>
        {
            op.Summary = "List questions";
            op.Description = "Returns a paginated list of all questions. Can be filtered by answer type or mandatory status.";
            return op;
        })
        .Produces<PaginatedResponse<Question>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/questions/{id} - Get single question
        questionGroup.MapGet("/{id:guid}", async (IQuestionRepository repo, Guid id) =>
        {
            var item = await repo.GetByIdAsync(id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetQuestion")
        .WithOpenApi(op =>
        {
            op.Summary = "Get question by ID";
            op.Description = "Returns a single question by its unique identifier.";
            return op;
        })
        .Produces<Question>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/questions/{id}/answers - Get answers for a question
        questionGroup.MapGet("/{id:guid}/answers", async (
            IQuestionRepository questionRepo,
            IAnswerRepository answerRepo,
            Guid id) =>
        {
            var question = await questionRepo.GetByIdAsync(id);
            if (question is null)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var answers = await answerRepo.GetByQuestionIdAsync(id);
            return Results.Ok(answers.ToList());
        })
        .WithName("GetQuestionAnswers")
        .WithOpenApi(op =>
        {
            op.Summary = "List answers for a question";
            op.Description = "Returns all answer options for a specific question.";
            return op;
        })
        .Produces<List<Answer>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/questions - Create new question
        questionGroup.MapPost("/", async (IQuestionRepository repo, CreateQuestionRequest request) =>
        {
            var newItem = await repo.CreateAsync(request);
            return Results.Created($"/api/v1/questions/{newItem.Id}", newItem);
        })
        .WithName("CreateQuestion")
        .WithOpenApi(op =>
        {
            op.Summary = "Create question";
            op.Description = "Creates a new question for a product or service.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Question>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/questions/{id} - Update question
        questionGroup.MapPut("/{id:guid}", async (IQuestionRepository repo, Guid id, UpdateQuestionRequest request) =>
        {
            var updated = await repo.UpdateAsync(id, request);
            return updated is not null
                ? Results.Ok(updated)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("UpdateQuestion")
        .WithOpenApi(op =>
        {
            op.Summary = "Update question";
            op.Description = "Updates an existing question. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Question>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/questions/{id} - Delete question
        questionGroup.MapDelete("/{id:guid}", async (IQuestionRepository repo, Guid id) =>
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted
                ? Results.NoContent()
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("DeleteQuestion")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete question";
            op.Description = "Deletes an existing question by its ID.";
            return op;
        })
        .RequireAuthorization()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // Answers endpoints
        var answerGroup = app.MapGroup("/api/v1/answers")
            .WithTags("Answers");

        // GET /api/v1/answers - List all answers
        answerGroup.MapGet("/", async (
            IAnswerRepository repo,
            int? page,
            int? pageSize,
            Guid? questionId,
            bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var all = await repo.GetAllAsync();
            var filtered = all.AsEnumerable();

            if (activeOnly == true) filtered = filtered.Where(x => x.IsActive);
            if (questionId.HasValue) filtered = filtered.Where(x => x.QuestionId == questionId);

            var list = filtered.OrderBy(x => x.DisplayOrder).ToList();
            var totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)currentPageSize);

            var items = list
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            return Results.Ok(new PaginatedResponse<Answer>(
                Items: items,
                Page: currentPage,
                PageSize: currentPageSize,
                TotalCount: totalCount,
                TotalPages: totalPages
            ));
        })
        .WithName("GetAnswers")
        .WithOpenApi(op =>
        {
            op.Summary = "List answers";
            op.Description = "Returns a paginated list of all answers. Can be filtered by question.";
            return op;
        })
        .Produces<PaginatedResponse<Answer>>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/v1/answers/{id} - Get single answer
        answerGroup.MapGet("/{id:guid}", async (IAnswerRepository repo, Guid id) =>
        {
            var item = await repo.GetByIdAsync(id);
            return item is not null
                ? Results.Ok(item)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Answer with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("GetAnswer")
        .WithOpenApi(op =>
        {
            op.Summary = "Get answer by ID";
            op.Description = "Returns a single answer by its unique identifier.";
            return op;
        })
        .Produces<Answer>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/v1/answers - Create new answer
        answerGroup.MapPost("/", async (
            IQuestionRepository questionRepo,
            IAnswerRepository answerRepo,
            CreateAnswerRequest request) =>
        {
            var question = await questionRepo.GetByIdAsync(request.QuestionId);
            if (question is null)
            {
                return Results.Problem(
                    title: "Bad Request",
                    detail: $"Question with ID '{request.QuestionId}' was not found.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var newItem = await answerRepo.CreateAsync(request);
            return Results.Created($"/api/v1/answers/{newItem.Id}", newItem);
        })
        .WithName("CreateAnswer")
        .WithOpenApi(op =>
        {
            op.Summary = "Create answer";
            op.Description = "Creates a new answer option for a question.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Answer>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // PUT /api/v1/answers/{id} - Update answer
        answerGroup.MapPut("/{id:guid}", async (IAnswerRepository repo, Guid id, UpdateAnswerRequest request) =>
        {
            var updated = await repo.UpdateAsync(id, request);
            return updated is not null
                ? Results.Ok(updated)
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Answer with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("UpdateAnswer")
        .WithOpenApi(op =>
        {
            op.Summary = "Update answer";
            op.Description = "Updates an existing answer. Only provided fields will be updated.";
            return op;
        })
        .RequireAuthorization()
        .Produces<Answer>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
        .Produces<ProblemDetails>(StatusCodes.Status403Forbidden)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // DELETE /api/v1/answers/{id} - Delete answer
        answerGroup.MapDelete("/{id:guid}", async (IAnswerRepository repo, Guid id) =>
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted
                ? Results.NoContent()
                : Results.Problem(
                    title: "Not Found",
                    detail: $"Answer with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
        })
        .WithName("DeleteAnswer")
        .WithOpenApi(op =>
        {
            op.Summary = "Delete answer";
            op.Description = "Deletes an existing answer by its ID.";
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
