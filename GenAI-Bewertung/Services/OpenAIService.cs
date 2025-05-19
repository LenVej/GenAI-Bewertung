using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GenAI_Bewertung.DTOs;

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

            
            if (rawMessage.Trim().StartsWith("{") && rawMessage.Contains("\\\""))
            {
                // Ent-escape den JSON-String
                try
                {
                    rawMessage = JsonSerializer.Deserialize<string>($"\"{rawMessage}\"");
                    rawMessage = rawMessage.Replace("\\\"", "\""); // Entfernt unnötige Escape-Zeichen
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fehler beim Entpacken des verschachtelten JSON:");
                    Console.WriteLine(e.Message);
                }
            }

            if (string.IsNullOrWhiteSpace(rawMessage)) return null;

            Console.WriteLine("Final JSON zum Parsen:");
            Console.WriteLine(rawMessage);

            var result = JsonSerializer.Deserialize<AiEvaluationResultDto>(rawMessage, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null)
            {
                Console.WriteLine("Deserialisierung fehlgeschlagen. RawMessage:");
                Console.WriteLine(rawMessage);
            }

            return result;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine("Fehler bei der Deserialisierung des Ergebnisses:");
            Console.WriteLine(jsonEx.Message);
            return null;
        }
    }
}
