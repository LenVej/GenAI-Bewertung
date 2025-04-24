using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Mappers;
using GenAI_Bewertung.Repositories;

namespace GenAI_Bewertung.Services;

public class ExamAttemptService
{
    private readonly IExamAttemptRepository _repo;

    public ExamAttemptService(IExamAttemptRepository repo)
    {
        _repo = repo;
    }

    public async Task<ExamAttempt?> StartAttemptAsync(int examId, int userId)
    {
        return await _repo.CreateAttemptAsync(examId, userId);
    }

    public async Task<ExamAttemptResultDto?> SubmitAttemptAsync(SubmitExamAttemptDto dto)
    {
        return await _repo.SaveAnswersAndEvaluateAsync(dto);
    }

    public async Task<ExamAttemptResultDto?> GetResultAsync(int attemptId, int userId)
    {
        var result = await _repo.GetAttemptResultAsync(attemptId, userId);
        return result;
    }
}