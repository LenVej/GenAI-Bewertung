namespace GenAI_Bewertung.Entities;

public class Question
{
    public int QuestionId { get; set; } //prime key
    public string QuestionText { get; set; }
    public string QuestionType { get; set; } // z.B. "MultipleChoice", "OneWord", etc.
    public string Subject { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}