namespace GenAI_Bewertung.Entities;

public class Answer
{
    public int AnswerId { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public string AnswerText { get; set; }
    public DateTime SubmittetAt { get; set; } = DateTime.UtcNow;
}
