using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Enums;

public class UpdateQuestionDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = "";
    public string Subject { get; set; } = "";
    public QuestionType QuestionType { get; set; }

    public List<string>? Choices { get; set; }
    public List<int>? CorrectIndices { get; set; }

    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? CorrectAnswer { get; set; }

    public string? ExpectedAnswer { get; set; }
    public double? ExpectedResult { get; set; }
    public double? CorrectValue { get; set; }

    public string? ClozeText { get; set; }
    public List<CreateBlankGapDto>? Gaps { get; set; }

    public string? ExpectedKeywords { get; set; }
}