namespace GenAI_Bewertung.Entities;

public class Exam
{
    public int ExamId  { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? TimeLimitMinutes { get; set; }

    public List<ExamQuestion> Questions { get; set; } = new();
}