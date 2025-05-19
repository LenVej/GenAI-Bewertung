namespace GenAI_Bewertung.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public string Tolerance { get; set; } = "medium"; // "low", "medium", "high"
    public bool CaseSensitive { get; set; } = false;
    public int EstimateTolerance { get; set; } = 10;
}   