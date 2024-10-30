using GenAI_Bewertung.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using GenAI_Bewertung.Repositories;

namespace GenAI_Bewertung.Services
{
    public class QuestionService
    {
        private readonly IQuestionRepository _repository;

        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Question>> GetAllQuestionsAsync() => _repository.GetQuestionsAsync();
        public Task<Question?> GetQuestionByIdAsync(int id) => _repository.GetQuestionByIdAsync(id);
        public Task AddQuestionAsync(Question question) => _repository.AddQuestionAsync(question);
        public Task UpdateQuestionAsync(Question question) => _repository.UpdateQuestionAsync(question);
        public Task DeleteQuestionAsync(Question question) => _repository.DeleteQuestionAsync(question);
        public Task<bool> QuestionExistsAsync(int id) => _repository.QuestionExistsAsync(id);
    }
}