using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Entities;

namespace GenAI_Bewertung.Services;

public class OpenAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = new HttpClient();
        _apiKey = config["OpenAI:ApiKey"]!;
    }

    public async Task<AiEvaluationResultDto?> EvaluateAsync(
        string question,
        string answer,
        List<string>? choices = null,
        List<string>? correctAnswers = null,
        string tolerance = "medium",
        bool caseSensitive = false,
        int estimateTolerance = 10)


    {
        var promptTemplate = choices == null
            ? """
              Du bist ein Lehrer. Bewerte die folgende Schülerantwort auf eine Frage. Gib ein Ergebnis als JSON zurück.

              Frage:
              {0}

              Antwort:
              {1}

              Lösung(en):
              {2}

              Rückgabeformat:
              {{
                "score": <0-1>,
                "feedback": "..."
              }}
              """
            : """
              Du bist ein Lehrer. Bewerte die folgende Schülerantwort auf eine Multiple-Choice-Frage. Die möglichen Antworten lauten:

              Auswahlmöglichkeiten:
              {3}

              Frage:
              {0}

              Antwort (Index oder Text):
              {1}

              Lösung(en):
              {2}

              Rückgabeformat:
              {{
                "score": <0-1>,
                "feedback": "..."
              }}
              """;
        
        var settingsInfo = $"""
                            Nutzereinstellungen – Bitte streng beachten:
                            - Tippfehler-Toleranz: {tolerance}
                            - Groß-/Kleinschreibung beachten: {(caseSensitive ? "Ja" : "Nein")}
                            - Schätztoleranz: ±{estimateTolerance}%

                            Anweisungen:
                            - Wenn Tippfehler erlaubt sind, bewerte kleine Schreibfehler nicht negativ.
                            - Falls Groß-/Kleinschreibung nicht relevant ist, ignoriere sie vollständig.
                            - Wenn es sich um eine Schätzfrage handelt, betrachte Werte innerhalb der angegebenen Toleranz noch als korrekt.
                            - Sei fair, aber auch präzise in der Bewertung. Gib immer Feedback, das zur Verbesserung hilft.

                            """;

        var correctAnswerText = correctAnswers != null
            ? string.Join(", ", correctAnswers)
            : "nicht angegeben";

        var choicesText = choices != null
            ? string.Join(", ", choices.Select((c, i) => $"{i}: {c}"))
            : "";

        var prompt = string.Format(promptTemplate, question, answer, correctAnswerText, choicesText);
        prompt = settingsInfo + prompt;

        Console.WriteLine("Prompt an GPT gesendet:");
        Console.WriteLine(prompt);

        var requestBody = new
        {
            model = "gpt-4",
            messages = new[]
            {
                new { role = "system", content = "Du bist ein Lehrer, der kurz, fair und objektiv bewertet." },
                new { role = "user", content = prompt }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine("GPT-Antwort:");
        Console.WriteLine(responseString);

        try
        {
            var root = JsonDocument.Parse(responseString);
            var rawMessage = root
                .RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(rawMessage)) return null;

            var match = Regex.Match(rawMessage, @"\{\s*""score""\s*:\s*\d+(\.\d+)?,\s*""feedback""\s*:\s*"".*?""\s*\}",
                RegexOptions.Singleline);
            var rightJson = match.Success ? match.Value : rawMessage.Trim();

            Console.WriteLine("Final JSON zum Parsen:");
            Console.WriteLine(rightJson);

            var result = JsonSerializer.Deserialize<AiEvaluationResultDto>(rightJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Deserialisieren:");
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}