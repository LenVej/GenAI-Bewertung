using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GenAI_Bewertung.Entities.QuestionTypes;

public class FillInTheBlankQuestion : Question
{
    public string ClozeText { get; set; } = string.Empty;
    public List<BlankGap> Gaps { get; set; } = new();
}

public class BlankGap
{
    [Key]
    public int Id { get; set; }

    public int Index { get; set; }

    public List<string> Solutions { get; set; } = new();

    
    public int FillInTheBlankQuestionId { get; set; }

    [ForeignKey("FillInTheBlankQuestionId")]
    
    [JsonIgnore] 
    public FillInTheBlankQuestion FillInTheBlankQuestion { get; set; }
}