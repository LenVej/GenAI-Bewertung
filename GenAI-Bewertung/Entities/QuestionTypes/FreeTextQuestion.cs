namespace GenAI_Bewertung.Entities.QuestionTypes;

public class FreeTextQuestion : Question
{
    public string ExpectedKeywords { get; set; } = string.Empty;
}