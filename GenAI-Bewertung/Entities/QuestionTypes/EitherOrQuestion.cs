namespace GenAI_Bewertung.Entities.QuestionTypes;

public class EitherOrQuestion : Question
{
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}