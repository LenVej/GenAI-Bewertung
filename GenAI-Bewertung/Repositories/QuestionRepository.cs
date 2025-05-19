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
            var baseQuestion = await _context.Questions
                .Where(q => q.QuestionId == id)
                .FirstOrDefaultAsync();

            if (baseQuestion is FillInTheBlankQuestion)
            {
                return await _context.Questions
                    .OfType<FillInTheBlankQuestion>()
                    .Include(q => q.Gaps)
                    .FirstOrDefaultAsync(q => q.QuestionId == id);
            }

            return baseQuestion;
        }



        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuestionAsync(Question updated)
        {
            var existing = await _context.Questions
                .Include(q => (q as FillInTheBlankQuestion)!.Gaps)
                .FirstOrDefaultAsync(q => q.QuestionId == updated.QuestionId);

            if (existing == null) return;

            _context.Entry(existing).CurrentValues.SetValues(updated);

            switch (updated)
            {
                case MultipleChoiceQuestion mc:
                    ((MultipleChoiceQuestion)existing).Choices = mc.Choices;
                    ((MultipleChoiceQuestion)existing).CorrectIndices = mc.CorrectIndices;
                    break;

                case EitherOrQuestion eo:
                    ((EitherOrQuestion)existing).OptionA = eo.OptionA;
                    ((EitherOrQuestion)existing).OptionB = eo.OptionB;
                    ((EitherOrQuestion)existing).CorrectAnswer = eo.CorrectAnswer;
                    break;

                case OneWordQuestion ow:
                    ((OneWordQuestion)existing).ExpectedAnswer = ow.ExpectedAnswer;
                    break;

                case MathQuestion mq:
                    ((MathQuestion)existing).ExpectedResult = mq.ExpectedResult;
                    break;

                case EstimationQuestion est:
                    ((EstimationQuestion)existing).CorrectValue = est.CorrectValue;
                    break;

                case FillInTheBlankQuestion fib:
                    var existingFib = (FillInTheBlankQuestion)existing;
                    existingFib.ClozeText = fib.ClozeText;

                    
                    var incomingGaps = fib.Gaps;
                    var existingGaps = existingFib.Gaps;

                    // 1. Aktualisiere existierende Gaps
                    foreach (var gap in incomingGaps)
                    {
                        var match = existingGaps.FirstOrDefault(g => g.Index == gap.Index);
                        if (match != null)
                        {
                            match.Solutions = gap.Solutions;
                        }
                        else
                        {
                            existingGaps.Add(new BlankGap
                            {
                                Index = gap.Index,
                                Solutions = gap.Solutions
                            });
                        }
                    }

                    // 2. Entferne Gaps, die nicht mehr vorkommen
                    var toRemove = existingGaps.Where(g => !incomingGaps.Any(ng => ng.Index == g.Index)).ToList();
                    _context.RemoveRange(toRemove);

                    break;


                case FreeTextQuestion ft:
                    ((FreeTextQuestion)existing).ExpectedKeywords = ft.ExpectedKeywords;
                    break;
            }

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