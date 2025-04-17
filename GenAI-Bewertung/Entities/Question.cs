using GenAI_Bewertung.Enums;

namespace GenAI_Bewertung.Entities;

public class Question
{
    public int QuestionId { get; set; } //prime key
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    public string Subject { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}