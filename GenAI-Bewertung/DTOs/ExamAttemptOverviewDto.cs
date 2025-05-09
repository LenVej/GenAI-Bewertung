namespace GenAI_Bewertung.DTOs;

public class ExamAttemptOverviewDto
{
    public int AttemptId { get; set; }
    public string ExamTitle { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
    public double Score { get; set; }
    public bool IsPassed { get; set; }
}