namespace GenAI_Bewertung.DTOs;

public class CreateBlankGapDto
{
    public int Index { get; set; }
    public List<string> Solutions { get; set; } = new();
}