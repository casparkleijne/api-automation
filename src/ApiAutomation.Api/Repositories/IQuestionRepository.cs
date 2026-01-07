using ApiAutomation.Api.Models;

namespace ApiAutomation.Api.Repositories;

public interface IQuestionRepository : ICodeRepository<Question, CreateQuestionRequest, UpdateQuestionRequest>
{
    Task<IEnumerable<Question>> GetByAnswerTypeIdAsync(Guid answerTypeId);
    Task<IEnumerable<Question>> GetMandatoryAsync();
    Task<IEnumerable<Question>> GetActiveAsync();
}

public interface IAnswerRepository : ICodeRepository<Answer, CreateAnswerRequest, UpdateAnswerRequest>
{
    Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId);
}
