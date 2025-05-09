namespace GenAI_Bewertung.DTOs;

public class AiEvaluationResultDto
{
    public double Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
}