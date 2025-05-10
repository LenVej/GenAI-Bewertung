using System.Security.Claims;
using GenAI_Bewertung.Data;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using GenAI_Bewertung.Mappers;
using GenAI_Bewertung.Services;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories;

public class ExamAttemptRepository : IExamAttemptRepository
{
    private readonly ApplicationDbContext _context;
    private readonly OpenAiService _openAi;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExamAttemptRepository(ApplicationDbContext context, OpenAiService openAi, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _openAi = openAi;
        _httpContextAccessor = httpContextAccessor;
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
        
        var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return null;

        var userId = int.Parse(userIdStr);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        var attempt = await _context.ExamAttempts
            .Include(a => a.Exam)
            .ThenInclude(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == dto.AttemptId);

        if (attempt == null || attempt.SubmittedAt != null)
            return null;

        var now = DateTime.UtcNow;
        var answerEntities = new List<ExamAnswer>();

        // Phase 1: Speichere Antworten ohne Evaluation
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

            answerEntities.Add(answer);
            _context.ExamAnswers.Add(answer);
        }

        // Save to get generated Answer IDs
        await _context.SaveChangesAsync();

        // Phase 2: Evaluationen nachträglich hinzufügen
        foreach (var answer in answerEntities)
        {
            var question = attempt.Exam.Questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId)?.Question;
            if (question == null) continue;

            List<string>? choices = null;
            List<string>? correctAnswers = null;
            string userAnswer;

            if (question is MultipleChoiceQuestion mcq)
            {
                choices = mcq.Choices;
                correctAnswers = mcq.CorrectIndices
                    .Select(i => i < mcq.Choices.Count ? mcq.Choices[i] : $"[Ungültig: {i}]").ToList();

                userAnswer = answer.SelectedIndices != null
                    ? string.Join(", ",
                        answer.SelectedIndices.Select(i => i < mcq.Choices.Count ? mcq.Choices[i] : $"[Ungültig: {i}]"))
                    : "Keine Auswahl";
            }
            else if (question is EitherOrQuestion eo)
            {
                choices = new List<string> { $"A: {eo.OptionA}", $"B: {eo.OptionB}" };

                correctAnswers = eo.CorrectAnswer.ToUpper() switch
                {
                    "A" => new List<string> { eo.OptionA },
                    "B" => new List<string> { eo.OptionB },
                    _ => null
                };

                userAnswer = answer.TextAnswer?.ToUpper() switch
                {
                    "A" => eo.OptionA,
                    "B" => eo.OptionB,
                    _ => answer.TextAnswer ?? "Keine Antwort"
                };
            }
            else
            {
                userAnswer = answer.TextAnswer ?? "Keine Antwort";
            }

            var eval = await _openAi.EvaluateAsync(
                question.QuestionText,
                userAnswer,
                choices,
                correctAnswers,
                user.Tolerance,
                user.CaseSensitive,
                user.EstimateTolerance
            );

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
        await _context.SaveChangesAsync();

        // Generiere Result-DTO
        var results = answerEntities.Select(ans =>
        {
            var question = attempt.Exam.Questions.First(q => q.QuestionId == ans.QuestionId).Question;
            var eitherOrForDto = question as EitherOrQuestion;

            return new AnswerResultDto
            {
                QuestionId = ans.QuestionId,
                QuestionText = question.QuestionText,
                TextAnswer = ans.TextAnswer,
                SelectedIndices = ans.SelectedIndices,
                AnswerChoices = question is MultipleChoiceQuestion mcq ? mcq.Choices : null,
                EitherOrOptionA = eitherOrForDto?.OptionA,
                EitherOrOptionB = eitherOrForDto?.OptionB,
                IsCorrect = ans.Evaluation?.IsCorrect ?? false,
                Score = ans.Evaluation?.Score ?? 0.0,
                Feedback = ans.Evaluation?.Feedback ?? "Keine Bewertung"
            };
        }).ToList();


        var scorePercent = results.Any() ? Math.Round(results.Average(r => r.Score) * 100, 2) : 0.0;

        var evaluationSummary = new ExamAttemptEvaluation
        {
            ExamAttemptId = attempt.ExamAttemptId,
            Score = scorePercent / 100.0,
            IsPassed = scorePercent >= 0.6,
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
            .Include(a => a.Answers)
            .ThenInclude(a => a.Question)
            .Include(a => a.Answers)
            .ThenInclude(a => a.Evaluation)
            .FirstOrDefaultAsync(a => a.ExamAttemptId == attemptId && a.UserId == userId);

        if (attempt == null || attempt.SubmittedAt == null) return null;

        var results = attempt.Answers.Select(ans =>
        {
            var question = ans.Question!;
            var choices = question is MultipleChoiceQuestion mcq ? mcq.Choices : null;
            var eitherOr = question as EitherOrQuestion;

            return new AnswerResultDto
            {
                QuestionId = ans.QuestionId,
                QuestionText = question.QuestionText,
                TextAnswer = ans.TextAnswer,
                SelectedIndices = ans.SelectedIndices,
                AnswerChoices = choices,
                EitherOrOptionA = eitherOr?.OptionA,
                EitherOrOptionB = eitherOr?.OptionB,
                IsCorrect = ans.Evaluation?.IsCorrect ?? false,
                Score = ans.Evaluation?.Score ?? 0.0,
                Feedback = ans.Evaluation?.Feedback ?? "Keine Auswertung"
            };
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

    public async Task<List<ExamAttempt>> GetCompletedAttemptsWithEvaluationAsync(int userId)
    {
        return await _context.ExamAttempts
            .Include(a => a.Exam)
            .Include(a => a.Evaluation)
            .Where(a => a.UserId == userId && a.SubmittedAt != null && a.Evaluation != null)
            .ToListAsync();
    }
}