using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Mappers;

public static class ExamAttemptMapper
{
    public static StartedExamAttemptDto ToStartedDto(ExamAttempt attempt)
    {
        return new StartedExamAttemptDto
        {
            AttemptId = attempt.ExamAttemptId,
            ExamId = attempt.ExamId,
            ExamTitle = attempt.Exam?.Title ?? "",
            TimeLimitMinutes = attempt.Exam?.TimeLimitMinutes,
            Questions = attempt.Exam?.Questions.OrderBy(q => q.Order).Select(eq => QuestionMapper.ToDto(eq.Question)).ToList() ?? new()
        };
    }

    public static ExamAttemptResultDto ToResultDto(ExamAttempt attempt)
    {
        var results = attempt.Answers.Select(a => new AnswerResultDto
        {
            QuestionId = a.QuestionId,
            QuestionText = a.Question?.QuestionText ?? "",
            TextAnswer = a.TextAnswer,
            SelectedIndices = a.SelectedIndices,
            IsCorrect = a.Evaluation?.IsCorrect ?? false,
            Score = a.Evaluation?.Score ?? 0,
            Feedback = a.Evaluation?.Feedback ?? ""
        }).ToList();

        var totalScore = results.Sum(r => r.Score);
        var maxScore = results.Count > 0 ? results.Count : 1;

        return new ExamAttemptResultDto
        {
            AttemptId = attempt.ExamAttemptId,
            UserId = attempt.UserId,
            ExamId = attempt.ExamId,
            ExamTitle = attempt.Exam?.Title ?? "",
            StartedAt = attempt.StartedAt,
            SubmittedAt = attempt.SubmittedAt ?? DateTime.UtcNow,
            Results = results,
            ScorePercent = Math.Round((totalScore / maxScore) * 100, 2)
        };
    }
}