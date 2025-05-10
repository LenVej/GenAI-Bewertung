namespace GenAI_Bewertung.DTOs;

public class AnswerResultDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = "";
    public string? TextAnswer { get; set; }
    public List<string>? AnswerChoices { get; set; }
    public List<int>? SelectedIndices { get; set; }
    public bool IsCorrect { get; set; }
    public double Score { get; set; }
    public string Feedback { get; set; } = "";
    
    public string? EitherOrOptionA { get; set; }
    public string? EitherOrOptionB { get; set; }
}