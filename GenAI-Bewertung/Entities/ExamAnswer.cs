namespace GenAI_Bewertung.Entities;

public class ExamAnswer
{
    public int ExamAnswerId { get; set; }

    public int ExamAttemptId { get; set; }
    public ExamAttempt ExamAttempt { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public string? TextAnswer { get; set; }          // For OneWord, FreeText, Estimation
    public List<int>? SelectedIndices { get; set; }  // For MultipleChoice
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;

    public AiEvaluationResult? Evaluation { get; set; }  // Optional
}
