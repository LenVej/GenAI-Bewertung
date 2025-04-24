namespace GenAI_Bewertung.Entities;

public class ExamAttempt
{
    public int ExamAttemptId { get; set; }
    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }

    public List<ExamAnswer> Answers { get; set; } = new();
}
