namespace GenAI_Bewertung.Entities.QuestionTypes;

public class OneWordQuestion : Question
{
    public string ExpectedAnswer { get; set; } = string.Empty;
}