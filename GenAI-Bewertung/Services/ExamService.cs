using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Repositories;

namespace GenAI_Bewertung.Services;

public class ExamService
{
    private readonly IExamRepository _repository;

    public ExamService(IExamRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Exam>> GetAllAsync() => _repository.GetAllAsync();
    public Task<Exam?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
    public Task<IEnumerable<Exam>> GetByUserIdAsync(int userId) => _repository.GetByUserIdAsync(userId);
    public Task AddAsync(Exam exam) => _repository.AddAsync(exam);
    public Task DeleteAsync(Exam exam) => _repository.DeleteAsync(exam);
    public Task UpdateAsync(Exam exam) => _repository.UpdateAsync(exam);

}
