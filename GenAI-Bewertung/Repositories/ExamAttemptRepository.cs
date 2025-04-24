using GenAI_Bewertung.Data;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Mappers;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories;

public class ExamAttemptRepository : IExamAttemptRepository
{
    private readonly ApplicationDbContext _context;

    public ExamAttemptRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExamAttempt?> CreateAttemptAsync(int examId, int userId)
    {
        var exam = await _context.Exams
            .Include(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(e => e.ExamId == examId);

        if (exam == null) return null;

        var attempt = new ExamAttempt
        {
            ExamId = examId,
            UserId = userId,
            StartedAt = DateTime.UtcNow
        };

        _context.ExamAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return await _context.ExamAttempts
            .Include(a => a.Exam)
            .ThenInclude(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == attempt.ExamAttemptId);
    }

    public async Task<ExamAttemptResultDto?> SaveAnswersAndEvaluateAsync(SubmitExamAttemptDto dto)
    {
        var attempt = await _context.ExamAttempts
            .Include(a => a.Exam)
            .ThenInclude(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == dto.AttemptId);

        if (attempt == null || attempt.SubmittedAt != null) return null;

        var now = DateTime.UtcNow;

        var answerEntities = dto.Answers.Select(a => new ExamAnswer
        {
            ExamAttemptId = attempt.ExamAttemptId,
            QuestionId = a.QuestionId,
            TextAnswer = a.TextAnswer,
            SelectedIndices = a.SelectedIndices,
            AnsweredAt = now
        }).ToList();

        _context.ExamAnswers.AddRange(answerEntities);
        await _context.SaveChangesAsync();

        attempt.SubmittedAt = now;
        await _context.SaveChangesAsync();

        var results = answerEntities.Select(ans => new AnswerResultDto
        {
            QuestionId = ans.QuestionId,
            QuestionText = attempt.Exam.Questions.First(q => q.QuestionId == ans.QuestionId).Question.QuestionText,
            TextAnswer = ans.TextAnswer,
            SelectedIndices = ans.SelectedIndices,
            IsCorrect = false, // ❗ Placeholder
            Score = 0.0,
            Feedback = "Feedback folgt nach AI-Auswertung"
        }).ToList();

        var resultDto = new ExamAttemptResultDto
        {
            AttemptId = attempt.ExamAttemptId,
            UserId = attempt.UserId,
            ExamId = attempt.ExamId,
            ExamTitle = attempt.Exam.Title,
            StartedAt = attempt.StartedAt,
            SubmittedAt = now,
            Results = results,
            ScorePercent = 0.0 // Berechnung folgt
        };

        return resultDto;
    }

    public async Task<ExamAttemptResultDto?> GetAttemptResultAsync(int attemptId, int userId)
    {
        var attempt = await _context.ExamAttempts
            .Include(a => a.Exam)
            .Include(a => a.Answers)
            .ThenInclude(a => a.Question)
            .Include(a => a.Answers)
            .ThenInclude(a => a.Evaluation)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == attemptId && a.UserId == userId);

        if (attempt == null || attempt.SubmittedAt == null) return null;

        var results = attempt.Answers.Select(ans => new AnswerResultDto
        {
            QuestionId = ans.QuestionId,
            QuestionText = ans.Question!.QuestionText,
            TextAnswer = ans.TextAnswer,
            SelectedIndices = ans.SelectedIndices,
            IsCorrect = ans.Evaluation?.IsCorrect ?? false,
            Score = ans.Evaluation?.Score ?? 0.0,
            Feedback = ans.Evaluation?.Feedback ?? "Keine Auswertung"
        }).ToList();

        var score = results.Any()
            ? results.Average(r => r.Score)
            : 0.0;

        return new ExamAttemptResultDto
        {
            AttemptId = attempt.ExamAttemptId,
            ExamId = attempt.ExamId,
            UserId = attempt.UserId,
            ExamTitle = attempt.Exam.Title,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt!.Value,
            Results = results,
            ScorePercent = score
        };
    }
}
