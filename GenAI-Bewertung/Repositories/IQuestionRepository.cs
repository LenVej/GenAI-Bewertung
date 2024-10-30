using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetQuestionsAsync();
    Task<Question?> GetQuestionByIdAsync(int id);
    Task AddQuestionAsync(Question question);
    Task UpdateQuestionAsync(Question question);
    Task DeleteQuestionAsync(Question question);
    Task<bool> QuestionExistsAsync(int id);
}