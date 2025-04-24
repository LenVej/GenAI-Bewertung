namespace GenAI_Bewertung.Entities;

public class AiEvaluationResult
{
    public int AiEvaluationResultId { get; set; }

    public int ExamAnswerId { get; set; }
    public ExamAnswer ExamAnswer { get; set; } = null!;

    public bool IsCorrect { get; set; }
    public double Score { get; set; }  // e.g. 0.0 - 1.0
    public string Feedback { get; set; } = "";
}
