using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GenAI_Bewertung.Tests.Service
{
    public class OpenAiServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public OpenAiServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private static OpenAiService CreateServiceWithResponse(string jsonResponse)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
            {
                { "OpenAI:ApiKey", "test-key" }
            }!).Build();

            return new OpenAiService(httpClient, config);
        }

        [Fact]
        public async Task EvaluateAsync_ReturnsParsedResult_WhenResponseIsValid()
        {
            var gptResponse = @"
            {
                ""choices"": [
                    {
                        ""message"": {
                            ""content"": ""{ \""score\"": 0.85, \""feedback\"": \""Gut beantwortet, aber kleine Fehler vorhanden.\""}""
                        }
                    }
                ]
            }";

            var service = CreateServiceWithResponse(gptResponse);
            var result = await service.EvaluateAsync("Was ist die Hauptstadt von Frankreich?", "Paris");

            Console.WriteLine("Returned result object:");
            Console.WriteLine($"Score: {result?.Score}");
            Console.WriteLine($"Feedback: {result?.Feedback}");

            Assert.NotNull(result);
            Assert.Equal(0.85, result!.Score);
            Assert.Contains("Gut beantwortet", result.Feedback);
        }

        [Fact]
        public async Task EvaluateAsync_ReturnsNull_WhenResponseIsEmpty()
        {
            var emptyResponse = @"{ ""choices"": [ { ""message"": { ""content"": """" } } ] }";
            var service = CreateServiceWithResponse(emptyResponse);

            var result = await service.EvaluateAsync("Frage", "Antwort");

            _testOutputHelper.WriteLine("Returned result (should be null): " + (result == null ? "null" : "NOT null"));

            Assert.Null(result);
        }

        [Fact]
        public async Task EvaluateAsync_ReturnsNull_WhenJsonInvalid()
        {
            var invalidJson = @"{ ""choices"": [ { ""message"": { ""content"": ""{ fehlerhaft }"" } } ] }";
            var service = CreateServiceWithResponse(invalidJson);

            var result = await service.EvaluateAsync("Frage", "Antwort");

            _testOutputHelper.WriteLine("Returned result (should be null): " + (result == null ? "null" : "NOT null"));

            Assert.Null(result);
        }

        [Fact]
        public async Task EvaluateAsync_HandlesMultipleChoiceScenario()
        {
            var innerJson = "{ \"score\": 1.0, \"feedback\": \"Perfekte Auswahl.\" }"; 
            var jsonResponse = $@"
    {{
        ""choices"": [
            {{
                ""message"": {{
                    ""content"": ""{innerJson}""
                }}
            }}
        ]
    }}";

            _testOutputHelper.WriteLine("=== MOCKED GPT JSON ===");
            _testOutputHelper.WriteLine(jsonResponse);

            var service = CreateServiceWithResponse(jsonResponse);

            var result = await service.EvaluateAsync(
                question: "Welche Farben sind primär?",
                answer: "Rot, Blau",
                choices: new List<string> { "Rot", "Grün", "Blau" },
                correctAnswers: new List<string> { "Rot", "Blau" }
            );

            _testOutputHelper.WriteLine("=== PARSED RESULT ===");
            _testOutputHelper.WriteLine($"Score: {result?.Score}");
            _testOutputHelper.WriteLine($"Feedback: {result?.Feedback}");

            Assert.NotNull(result);
            Assert.Equal(1.0, result!.Score);
            Assert.Contains("Perfekte", result.Feedback);
        }

    }
}
