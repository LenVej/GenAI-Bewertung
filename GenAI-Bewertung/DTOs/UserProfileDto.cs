namespace GenAI_Bewertung.DTOs;

public class UserProfileDto
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public string Tolerance { get; set; } = "medium";
    public bool CaseSensitive { get; set; } = false;
    public int EstimateTolerance { get; set; } = 10;
}
