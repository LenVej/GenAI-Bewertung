namespace GenAI_Bewertung.DTOs;

public class SubmittedAnswerDto
{
    public int QuestionId { get; set; }
    public string? TextAnswer { get; set; }
    public List<int>? SelectedIndices { get; set; }
}