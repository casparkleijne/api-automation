using ApiAutomation.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiAutomation.Api.Endpoints;

public static class QuestionEndpoints
{
    // In-memory storage for demo purposes
    private static readonly List<Question> _questions = new()
    {
        new Question(
            Id: Guid.Parse("99999999-9999-9999-9999-999999999991"),
            Name: "Connection Capacity",
            Description: "Question about connection capacity",
            WebsiteLink: null,
            WebsiteText: null,
            Placeholder: "Select capacity",
            Code: "Q001",
            Priority: 1,
            QuestionTitle: "Connection Capacity",
            QuestionText: "What is the requested connection capacity?",
            ResultFormat: "select",
            HasAction: false,
            IsEnabled: true,
            AnswerHandler: 1,
            IsMandatory: true,
            AnswerTypeId: null,
            ShowForAddressableObjects: true,
            ShowForNonAddressableObjects: false,
            ShowForAddressableObjectsOnly: false,
            QuestionRequestTypeId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Question(
            Id: Guid.Parse("99999999-9999-9999-9999-999999999992"),
            Name: "Installation Date",
            Description: "Question about preferred installation date",
            WebsiteLink: null,
            WebsiteText: null,
            Placeholder: "Select date",
            Code: "Q002",
            Priority: 2,
            QuestionTitle: "Installation Date",
            QuestionText: "What is the preferred installation date?",
            ResultFormat: "date",
            HasAction: false,
            IsEnabled: true,
            AnswerHandler: 2,
            IsMandatory: true,
            AnswerTypeId: null,
            ShowForAddressableObjects: true,
            ShowForNonAddressableObjects: true,
            ShowForAddressableObjectsOnly: false,
            QuestionRequestTypeId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        ),
        new Question(
            Id: Guid.Parse("99999999-9999-9999-9999-999999999993"),
            Name: "Temporary Power",
            Description: "Question about temporary power supply",
            WebsiteLink: null,
            WebsiteText: null,
            Placeholder: null,
            Code: "Q003",
            Priority: 3,
            QuestionTitle: "Temporary Power Supply",
            QuestionText: "Do you need a temporary power supply during construction?",
            ResultFormat: "boolean",
            HasAction: false,
            IsEnabled: true,
            AnswerHandler: 3,
            IsMandatory: false,
            AnswerTypeId: null,
            ShowForAddressableObjects: true,
            ShowForNonAddressableObjects: true,
            ShowForAddressableObjectsOnly: false,
            QuestionRequestTypeId: null,
            IsActive: true,
            StartDate: "2025-01-01T00:00:00.000Z",
            EndDate: "2099-12-31T23:59:59.999Z"
        )
    };

    private static readonly List<Answer> _answers = new()
    {
        new Answer(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Code: "A001_1",
            Text: "1x25A (Single phase)",
            Description: "Suitable for small apartments",
            QuestionId: Guid.Parse("99999999-9999-9999-9999-999999999991"),
            DisplayOrder: 1,
            IsDefault: false,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Answer(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab1"),
            Code: "A001_2",
            Text: "3x25A (Three phase)",
            Description: "Standard for most homes",
            QuestionId: Guid.Parse("99999999-9999-9999-9999-999999999991"),
            DisplayOrder: 2,
            IsDefault: true,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        ),
        new Answer(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaab2"),
            Code: "A001_3",
            Text: "3x35A (Three phase)",
            Description: "For homes with heat pump or EV charger",
            QuestionId: Guid.Parse("99999999-9999-9999-9999-999999999991"),
            DisplayOrder: 3,
            IsDefault: false,
            IsActive: true,
            CreatedAt: "2025-01-01T00:00:00.000Z",
            ModifiedAt: null
        )
    };

    public static void MapQuestionEndpoints(this WebApplication app)
    {
        var questionGroup = app.MapGroup("/api/v1/questions")
            .WithTags("Questions");

        // GET /api/v1/questions - List all questions
        questionGroup.MapGet("/", (int? page, int? pageSize, Guid? answerTypeId, bool? activeOnly, bool? mandatoryOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _questions.AsEnumerable();
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
        questionGroup.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _questions.FirstOrDefault(x => x.Id == id);
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
        questionGroup.MapGet("/{id:guid}/answers", (Guid id) =>
        {
            if (!_questions.Any(x => x.Id == id))
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var answers = _answers
                .Where(x => x.QuestionId == id)
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            return Results.Ok(answers);
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
        questionGroup.MapPost("/", (CreateQuestionRequest request) =>
        {
            var newItem = new Question(
                Id: Guid.NewGuid(),
                Name: request.Name,
                Description: request.Description,
                WebsiteLink: request.WebsiteLink,
                WebsiteText: request.WebsiteText,
                Placeholder: request.Placeholder,
                Code: request.Code,
                Priority: request.Priority,
                QuestionTitle: request.QuestionTitle,
                QuestionText: request.QuestionText,
                ResultFormat: request.ResultFormat,
                HasAction: request.HasAction,
                IsEnabled: request.IsEnabled,
                AnswerHandler: request.AnswerHandler,
                IsMandatory: request.IsMandatory,
                AnswerTypeId: request.AnswerTypeId,
                ShowForAddressableObjects: request.ShowForAddressableObjects,
                ShowForNonAddressableObjects: request.ShowForNonAddressableObjects,
                ShowForAddressableObjectsOnly: request.ShowForAddressableObjectsOnly,
                QuestionRequestTypeId: request.QuestionRequestTypeId,
                IsActive: true,
                StartDate: request.StartDate,
                EndDate: request.EndDate
            );

            _questions.Add(newItem);
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
        questionGroup.MapPut("/{id:guid}", (Guid id, UpdateQuestionRequest request) =>
        {
            var index = _questions.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _questions[index];
            var updated = existing with
            {
                Name = request.Name ?? existing.Name,
                Description = request.Description ?? existing.Description,
                WebsiteLink = request.WebsiteLink ?? existing.WebsiteLink,
                WebsiteText = request.WebsiteText ?? existing.WebsiteText,
                Placeholder = request.Placeholder ?? existing.Placeholder,
                Code = request.Code ?? existing.Code,
                Priority = request.Priority ?? existing.Priority,
                QuestionTitle = request.QuestionTitle ?? existing.QuestionTitle,
                QuestionText = request.QuestionText ?? existing.QuestionText,
                ResultFormat = request.ResultFormat ?? existing.ResultFormat,
                HasAction = request.HasAction ?? existing.HasAction,
                IsEnabled = request.IsEnabled ?? existing.IsEnabled,
                AnswerHandler = request.AnswerHandler ?? existing.AnswerHandler,
                IsMandatory = request.IsMandatory ?? existing.IsMandatory,
                AnswerTypeId = request.AnswerTypeId ?? existing.AnswerTypeId,
                ShowForAddressableObjects = request.ShowForAddressableObjects ?? existing.ShowForAddressableObjects,
                ShowForNonAddressableObjects = request.ShowForNonAddressableObjects ?? existing.ShowForNonAddressableObjects,
                ShowForAddressableObjectsOnly = request.ShowForAddressableObjectsOnly ?? existing.ShowForAddressableObjectsOnly,
                QuestionRequestTypeId = request.QuestionRequestTypeId ?? existing.QuestionRequestTypeId,
                IsActive = request.IsActive ?? existing.IsActive,
                StartDate = request.StartDate ?? existing.StartDate,
                EndDate = request.EndDate ?? existing.EndDate
            };

            _questions[index] = updated;
            return Results.Ok(updated);
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
        questionGroup.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _questions.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Question with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _questions.RemoveAt(index);
            return Results.NoContent();
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
        answerGroup.MapGet("/", (int? page, int? pageSize, Guid? questionId, bool? activeOnly) =>
        {
            var currentPage = Math.Max(1, page ?? 1);
            var currentPageSize = Math.Clamp(pageSize ?? 20, 1, 100);

            var filtered = _answers.AsEnumerable();
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
        answerGroup.MapGet("/{id:guid}", (Guid id) =>
        {
            var item = _answers.FirstOrDefault(x => x.Id == id);
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
        answerGroup.MapPost("/", (CreateAnswerRequest request) =>
        {
            if (!_questions.Any(x => x.Id == request.QuestionId))
            {
                return Results.Problem(
                    title: "Bad Request",
                    detail: $"Question with ID '{request.QuestionId}' was not found.",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var newItem = new Answer(
                Id: Guid.NewGuid(),
                Code: request.Code,
                Text: request.Text,
                Description: request.Description,
                QuestionId: request.QuestionId,
                DisplayOrder: request.DisplayOrder,
                IsDefault: request.IsDefault,
                IsActive: true,
                CreatedAt: now,
                ModifiedAt: null
            );

            _answers.Add(newItem);
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
        answerGroup.MapPut("/{id:guid}", (Guid id, UpdateAnswerRequest request) =>
        {
            var index = _answers.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Answer with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            var existing = _answers[index];
            var now = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var updated = existing with
            {
                Code = request.Code ?? existing.Code,
                Text = request.Text ?? existing.Text,
                Description = request.Description ?? existing.Description,
                DisplayOrder = request.DisplayOrder ?? existing.DisplayOrder,
                IsDefault = request.IsDefault ?? existing.IsDefault,
                IsActive = request.IsActive ?? existing.IsActive,
                ModifiedAt = now
            };

            _answers[index] = updated;
            return Results.Ok(updated);
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
        answerGroup.MapDelete("/{id:guid}", (Guid id) =>
        {
            var index = _answers.FindIndex(x => x.Id == id);
            if (index < 0)
            {
                return Results.Problem(
                    title: "Not Found",
                    detail: $"Answer with ID '{id}' was not found.",
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            _answers.RemoveAt(index);
            return Results.NoContent();
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
