using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Mappers;

public static class ExamMapper
{
    public static ExamDto ToDto(Exam exam)
    {
        return new ExamDto
        {
            ExamId = exam.ExamId,
            Title = exam.Title,
            Description = exam.Description,
            TimeLimitMinutes = exam.TimeLimitMinutes,
            CreatedBy = exam.CreatedBy,
            CreatedAt = exam.CreatedAt,
            Questions = exam.Questions
                .OrderBy(q => q.Order)
                .Where(eq => eq.Question != null)
                .Select(eq => QuestionMapper.ToDto(eq.Question))
                .ToList()
        };
    }

    public static Exam FromCreateDto(CreateExamDto dto, int userId)
    {
        return new Exam
        {
            Title = dto.Title,
            Description = dto.Description,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            Questions = dto.QuestionIds.Select((qid, index) => new ExamQuestion
            {
                QuestionId = qid,
                Order = index
            }).ToList()
        };
    }
    
    public static void UpdateFromDto(Exam exam, UpdateExamDto dto)
    {
        exam.Title = dto.Title;
        exam.Description = dto.Description;
        exam.TimeLimitMinutes = dto.TimeLimitMinutes;
    
        
        exam.Questions = dto.QuestionIds.Select((id, idx) => new ExamQuestion
        {
            QuestionId = id,
            Order = idx
        }).ToList();
    }
}