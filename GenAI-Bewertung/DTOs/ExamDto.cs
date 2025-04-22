namespace GenAI_Bewertung.DTOs;

public class ExamDto
{
    public int ExamId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int? TimeLimitMinutes { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}