using System.Security.Claims;
using GenAI_Bewertung.Data;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Services;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories;

public class ExamAttemptRepository : IExamAttemptRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ExamScoringService _scoring;
    private readonly IHttpContextAccessor _http;

    public ExamAttemptRepository(ApplicationDbContext context, ExamScoringService scoring, IHttpContextAccessor http)
    {
        _context = context;
        _scoring = scoring;
        _http = http;
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
    var userIdStr = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userIdStr)) return null;

    var userId = int.Parse(userIdStr);
    var user = await _context.Users.FindAsync(userId);
    if (user == null) return null;

    var attempt = await _context.ExamAttempts
        .Include(a => a.Exam)
        .ThenInclude(e => e.Questions)
        .ThenInclude(eq => eq.Question)
        .FirstOrDefaultAsync(a => a.ExamAttemptId == dto.AttemptId);

    if (attempt == null || attempt.SubmittedAt != null) return null;

    
    foreach (var eq in attempt.Exam.Questions)
    {
        if (eq.Question is FillInTheBlankQuestion fib)
        {
            await _context.Entry(fib)
                .Collection(f => f.Gaps)
                .LoadAsync();
        }
    }

    var now = DateTime.UtcNow;
    var answers = new List<ExamAnswer>();

    foreach (var submitted in dto.Answers)
    {
        var question = attempt.Exam.Questions.FirstOrDefault(q => q.QuestionId == submitted.QuestionId)?.Question;
        if (question == null) continue;

        var answer = new ExamAnswer
        {
            ExamAttemptId = attempt.ExamAttemptId,
            QuestionId = submitted.QuestionId,
            TextAnswer = submitted.TextAnswer,
            SelectedIndices = submitted.SelectedIndices,
            AnsweredAt = now
        };

        _context.ExamAnswers.Add(answer);
        answers.Add(answer);
    }

    await _context.SaveChangesAsync();

    foreach (var answer in answers)
    {
        var question = attempt.Exam.Questions.First(q => q.QuestionId == answer.QuestionId).Question;

        var eval = await _scoring.ScoreAsync(question, answer, user);
        if (eval != null)
        {
            var evaluation = new AiEvaluationResult
            {
                ExamAnswerId = answer.ExamAnswerId,
                Feedback = eval.Feedback,
                Score = eval.Score,
                IsCorrect = eval.Score >= 0.6
            };

            _context.AiEvaluationResults.Add(evaluation);
            answer.Evaluation = evaluation;
        }
    }

    attempt.SubmittedAt = now;

    var results = answers.Select(ans =>
    {
        var question = attempt.Exam.Questions.First(q => q.QuestionId == ans.QuestionId).Question;
        var eitherOr = question as EitherOrQuestion;

        return new AnswerResultDto
        {
            QuestionId = ans.QuestionId,
            QuestionText = question.QuestionText,
            TextAnswer = ans.TextAnswer,
            SelectedIndices = ans.SelectedIndices,
            AnswerChoices = (question as MultipleChoiceQuestion)?.Choices,
            EitherOrOptionA = eitherOr?.OptionA,
            EitherOrOptionB = eitherOr?.OptionB,
            IsCorrect = ans.Evaluation?.IsCorrect ?? false,
            Score = ans.Evaluation?.Score ?? 0.0,
            Feedback = ans.Evaluation?.Feedback ?? "Keine Bewertung"
        };
    }).ToList();

    var scorePercent = results.Any() ? Math.Round(results.Average(r => r.Score) * 100, 2) : 0;

    var evaluationSummary = new ExamAttemptEvaluation
    {
        ExamAttemptId = attempt.ExamAttemptId,
        Score = scorePercent / 100.0,
        IsPassed = scorePercent >= 60,
        FeedbackSummary = "Guter Versuch, aber es gibt noch Verbesserungspotenzial.",
        EvaluatedAt = now
    };

    _context.ExamAttemptEvaluations.Add(evaluationSummary);
    await _context.SaveChangesAsync();

    return new ExamAttemptResultDto
    {
        AttemptId = attempt.ExamAttemptId,
        UserId = attempt.UserId,
        ExamId = attempt.ExamId,
        ExamTitle = attempt.Exam.Title,
        StartedAt = attempt.StartedAt,
        SubmittedAt = now,
        Results = results,
        ScorePercent = scorePercent
    };
}


    public async Task<ExamAttemptResultDto?> GetAttemptResultAsync(int attemptId, int userId)
    {
        var attempt = await _context.ExamAttempts
            .Include(a => a.Exam)
            .Include(a => a.Answers).ThenInclude(a => a.Question)
            .Include(a => a.Answers).ThenInclude(a => a.Evaluation)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == attemptId && a.UserId == userId);

        if (attempt == null || attempt.SubmittedAt == null) return null;

        var results = attempt.Answers.Select(ans =>
        {
            var question = ans.Question!;
            var eitherOr = question as EitherOrQuestion;

            return new AnswerResultDto
            {
                QuestionId = ans.QuestionId,
                QuestionText = question.QuestionText,
                TextAnswer = ans.TextAnswer,
                SelectedIndices = ans.SelectedIndices,
                AnswerChoices = (question as MultipleChoiceQuestion)?.Choices,
                EitherOrOptionA = eitherOr?.OptionA,
                EitherOrOptionB = eitherOr?.OptionB,
                IsCorrect = ans.Evaluation?.IsCorrect ?? false,
                Score = ans.Evaluation?.Score ?? 0.0,
                Feedback = ans.Evaluation?.Feedback ?? "Keine Auswertung"
            };
        }).ToList();

        var avgScore = results.Any() ? results.Average(r => r.Score) : 0.0;

        return new ExamAttemptResultDto
        {
            AttemptId = attempt.ExamAttemptId,
            ExamId = attempt.ExamId,
            UserId = attempt.UserId,
            ExamTitle = attempt.Exam.Title,
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt!.Value,
            Results = results,
            ScorePercent = avgScore
        };
    }

    public async Task<List<ExamAttempt>> GetCompletedAttemptsWithEvaluationAsync(int userId)
    {
        return await _context.ExamAttempts
            .Include(a => a.Exam)
            .Include(a => a.Evaluation)
            .Where(a => a.UserId == userId && a.SubmittedAt != null && a.Evaluation != null)
            .ToListAsync();
    }
}