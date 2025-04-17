namespace GenAI_Bewertung.Entities.QuestionTypes;

public class FillInTheBlankQuestion : Question
{
    public string ClozeText { get; set; } = string.Empty; // mit {{gap}} Markierungen
    public List<string> Solutions { get; set; } = new(); // mögliche Wörter
}