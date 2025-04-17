namespace GenAI_Bewertung.Entities.QuestionTypes;

public class MultipleChoiceQuestion : Question
{
    public List<string> Choices { get; set; } = new();
    public List<int> CorrectIndices { get; set; } = new();
}