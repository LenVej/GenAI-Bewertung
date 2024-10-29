namespace GenAI_Bewertung.Models;

public class Question
{
    public int Id { get; set; } //prime key
    public string Text { get; set; }
    public string Type { get; set; } // z.B. "MultipleChoice", "OneWord", etc.
}