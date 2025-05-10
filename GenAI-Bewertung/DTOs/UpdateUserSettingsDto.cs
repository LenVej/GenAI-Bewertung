namespace GenAI_Bewertung.DTOs;

public class UpdateUserSettingsDto
{
    public string Tolerance { get; set; } = "medium";
    public bool CaseSensitive { get; set; }
    public int EstimateTolerance { get; set; }
}