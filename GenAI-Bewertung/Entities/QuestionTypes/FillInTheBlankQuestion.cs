using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenAI_Bewertung.Entities.QuestionTypes;

public class FillInTheBlankQuestion : Question
{
    public string ClozeText { get; set; } = string.Empty; // z. B. "Die Hauptstadt von {{0}} ist {{1}}."
    public List<BlankGap> Gaps { get; set; } = new();
}

public class BlankGap
{
    [Key]
    public int Id { get; set; }

    public int Index { get; set; } // z.B. 0 für {{0}}, 1 für {{1}}, usw.

    public List<string> Solutions { get; set; } = new();

    // EF Core relationship
    public int FillInTheBlankQuestionId { get; set; }

    [ForeignKey("FillInTheBlankQuestionId")]
    public FillInTheBlankQuestion FillInTheBlankQuestion { get; set; }
}