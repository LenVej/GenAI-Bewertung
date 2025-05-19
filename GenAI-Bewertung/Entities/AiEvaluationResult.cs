namespace GenAI_Bewertung.Entities;

public class AiEvaluationResult
{
    public int AiEvaluationResultId { get; set; }
    public int ExamAnswerId { get; set; }
    public ExamAnswer ExamAnswer { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public double Score { get; set; }
    public string Feedback { get; set; } = "";
}
