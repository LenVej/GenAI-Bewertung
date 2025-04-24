using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Repositories;

public interface IExamAttemptRepository
{
    Task<ExamAttempt?> CreateAttemptAsync(int examId, int userId);
    Task<ExamAttemptResultDto?> SaveAnswersAndEvaluateAsync(SubmitExamAttemptDto dto);
    Task<ExamAttemptResultDto?> GetAttemptResultAsync(int attemptId, int userId);
}