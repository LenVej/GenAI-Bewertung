namespace GenAI_Bewertung.DTOs;

public class CreateExamDto 
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int? TimeLimitMinutes { get; set; }
    public List<int> QuestionIds { get; set; } = new();
}
