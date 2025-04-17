namespace GenAI_Bewertung.Entities.QuestionTypes;

public class EstimationQuestion : Question
{
    public double CorrectValue { get; set; }
    public double TolerancePercent { get; set; }
}