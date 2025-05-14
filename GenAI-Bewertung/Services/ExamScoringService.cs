using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;

namespace GenAI_Bewertung.Services;

public class ExamScoringService
{
    private readonly OpenAiService _openAi;

    public ExamScoringService(OpenAiService openAi)
    {
        _openAi = openAi;
    }

    public async Task<AiEvaluationResultDto?> ScoreAsync(Question question, ExamAnswer answer, User user)
{
    List<string>? choices = null;
    List<string>? correctAnswers = null;
    string userAnswer;
    string questionText = question.QuestionText;

    switch (question)
    {
        case MultipleChoiceQuestion mcq:
            choices = mcq.Choices;
            correctAnswers = mcq.CorrectIndices
                .Select(i => i < mcq.Choices.Count ? mcq.Choices[i] : "[Ungültig]").ToList();

            userAnswer = answer.SelectedIndices != null
                ? string.Join(", ",
                    answer.SelectedIndices.Select(i => i < mcq.Choices.Count ? mcq.Choices[i] : "[Ungültig]"))
                : "Keine Auswahl";
            break;

        case EitherOrQuestion eo:
            choices = new List<string> { $"A: {eo.OptionA}", $"B: {eo.OptionB}" };
            correctAnswers = eo.CorrectAnswer.ToUpper() switch
            {
                "A" => new List<string> { eo.OptionA },
                "B" => new List<string> { eo.OptionB },
                _ => new List<string> { "[Unbekannt]" }
            };

            userAnswer = answer.TextAnswer?.ToUpper() switch
            {
                "A" => eo.OptionA,
                "B" => eo.OptionB,
                _ => answer.TextAnswer ?? "Keine Antwort"
            };
            break;

        case OneWordQuestion ow:
            correctAnswers = new List<string> { ow.ExpectedAnswer };
            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            break;

        case MathQuestion m:
            correctAnswers = new List<string> { m.ExpectedResult.ToString("0.#####") };
            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            break;

        case EstimationQuestion est:
            correctAnswers = new List<string> { est.CorrectValue.ToString("0.#####") };
            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            break;

        case FillInTheBlankQuestion fib:
            // Nutze ClozeText als Frage
            questionText = fib.ClozeText;

            // Sammle alle Lückenlösungen
            correctAnswers = fib.Gaps
                .Select(g =>
                    $"Lücke {g.Index}: {string.Join(" / ", g.Solutions.Where(s => !string.IsNullOrWhiteSpace(s)))}"
                )
                .ToList();

            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            break;

        case FreeTextQuestion ft:
            correctAnswers = ft.ExpectedKeywords.Split(',').Select(s => s.Trim()).ToList();
            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            break;

        default:
            userAnswer = answer.TextAnswer ?? "Keine Antwort";
            correctAnswers = new List<string> { "Nicht definiert" };
            break;
    }

    return await _openAi.EvaluateAsync(
        questionText,
        userAnswer,
        choices,
        correctAnswers,
        user.Tolerance,
        user.CaseSensitive,
        user.EstimateTolerance
    );
}

}
