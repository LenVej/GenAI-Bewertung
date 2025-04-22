using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Repositories;

public interface IExamRepository
{
    Task<IEnumerable<Exam>> GetAllAsync();
    Task<Exam?> GetByIdAsync(int id);
    Task AddAsync(Exam exam);
    Task DeleteAsync(Exam exam);
    Task<IEnumerable<Exam>> GetByUserIdAsync(int userId);
    Task UpdateAsync(Exam exam);

}