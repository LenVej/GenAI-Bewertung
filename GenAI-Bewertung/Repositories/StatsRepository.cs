using GenAI_Bewertung.Data;
using GenAI_Bewertung.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly ApplicationDbContext _context;

    public StatsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProfileStatsDto> GetStatsForUserAsync(int userId)
    {
        var answers = await _context.ExamAnswers
            .AsNoTracking()
            .Include(a => a.Evaluation)
            .Include(a => a.Question)
            .Where(a => a.ExamAttempt.UserId == userId && a.Evaluation != null)
            .ToListAsync();

        var total = answers.Count;
        var correct = answers.Count(a => a.Evaluation!.IsCorrect);
        var incorrect = total - correct;

        var avgScore = total > 0
            ? answers.Average(a => a.Evaluation!.Score) * 100
            : 0;

        var grouped = answers
            .GroupBy(a => a.Question!.Subject)
            .Select(g => new SubjectWeaknessDto
            {
                Subject = g.Key ?? "Unbekannt",
                TotalQuestions = g.Count(),
                IncorrectAnswers = g.Count(a => !a.Evaluation!.IsCorrect)
            })
            .Where(s => s.IncorrectAnswers > 0)
            .ToList();


        return new ProfileStatsDto
        {
            AverageScorePercent = Math.Round(avgScore, 2),
            TotalCorrect = correct,
            TotalIncorrect = incorrect,
            WeakSubjects = grouped
        };
    }
}