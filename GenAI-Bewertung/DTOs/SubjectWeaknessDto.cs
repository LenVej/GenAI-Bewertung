namespace GenAI_Bewertung.DTOs;

public class SubjectWeaknessDto
{
    public string Subject { get; set; } = string.Empty;
    public int TotalQuestions { get; set; }
    public int IncorrectAnswers { get; set; }
}