namespace GenAI_Bewertung.Entities;

public class ExamQuestion {
    public int Id { get; set; }
    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int Order { get; set; }
}

