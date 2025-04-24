namespace GenAI_Bewertung.DTOs;

public class ExamAttemptResultDto
{
    public int AttemptId { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public string ExamTitle { get; set; } = "";
    public DateTime StartedAt { get; set; }
    public DateTime SubmittedAt { get; set; }
    public List<AnswerResultDto> Results { get; set; } = new();
    public double ScorePercent { get; set; }
}