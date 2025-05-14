using GenAI_Bewertung.Data;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync()
        {
            var questions = await _context.Questions.ToListAsync();

            foreach (var q in questions)
            {
                if (q is FillInTheBlankQuestion fib)
                {
                    await _context.Entry(fib)
                        .Collection(f => f.Gaps)
                        .LoadAsync();
                }
            }

            return questions;
        }


        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question question)
        {
            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(Question question)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> QuestionExistsAsync(int id)
        {
            return await _context.Questions.AnyAsync(e => e.QuestionId == id);
        }
        
        public async Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            return await _context.Questions.Where(q => q.CreatedBy == userId).ToListAsync();
        }

    }
}