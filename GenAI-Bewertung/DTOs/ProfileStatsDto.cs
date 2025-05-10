namespace GenAI_Bewertung.DTOs;

public class ProfileStatsDto
{
    public double AverageScorePercent { get; set; }
    public int TotalCorrect { get; set; }
    public int TotalIncorrect { get; set; }
    public List<SubjectWeaknessDto> WeakSubjects { get; set; } = new();
}