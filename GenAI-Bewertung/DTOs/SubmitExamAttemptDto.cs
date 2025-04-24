namespace GenAI_Bewertung.DTOs;

public class SubmitExamAttemptDto
{
    public int AttemptId { get; set; }
    public List<SubmittedAnswerDto> Answers { get; set; } = new();
}