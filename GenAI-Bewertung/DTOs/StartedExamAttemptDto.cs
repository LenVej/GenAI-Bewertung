namespace GenAI_Bewertung.DTOs;

public class StartedExamAttemptDto
{
    public int AttemptId { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; } = "";
    public int? TimeLimitMinutes { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}