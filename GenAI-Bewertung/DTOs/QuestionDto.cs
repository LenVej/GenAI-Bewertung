using GenAI_Bewertung.Enums;

namespace GenAI_Bewertung.DTOs;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = "";
    public QuestionType QuestionType { get; set; }
    public string Subject { get; set; } = "";
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Multiple Choice
    public List<string>? Choices { get; set; }
    public List<int>? CorrectIndices { get; set; }

    // EitherOr
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? CorrectAnswer { get; set; }

    // OneWord
    public string? ExpectedAnswer { get; set; }

    // Math
    public double? ExpectedResult { get; set; }

    // Estimation
    public double? CorrectValue { get; set; }

    // FillInTheBlank
    public string? ClozeText { get; set; }
    public List<BlankGapDto>? Gaps { get; set; }

    // FreeText
    public string? ExpectedKeywords { get; set; }
}