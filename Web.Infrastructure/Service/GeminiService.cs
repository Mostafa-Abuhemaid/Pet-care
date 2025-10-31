using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Application.Interfaces;
using Web.Domain.Entites;
using Web.Domain.Enums;

namespace Web.Infrastructure.Service
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(IConfiguration configuration, ILogger<GeminiService> logger)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new Exception("Gemini API Key not found");
            _logger = logger;
        }

        public async Task<string> GeneratePetAdvice(PetType petType, string breed, string petName)
        {
            var prompt = BuildPrompt(petType, breed, petName);  

            try
            {
                 var request=BuildRequest(prompt);
                var response = await _httpClient.PostAsync(request.url, request.content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Gemini API Error: {error}");
                    return "Unable to generate advice at this time.";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent);

                return geminiResponse?.Candidates?[0]?.Content?.Parts?[0]?.Text?.Trim()
                    ?? "Unable to generate advice.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Gemini API");
                return "An error occurred while generating advice.";
            }
        }

        private string BuildPrompt(PetType petType, string breed, string petName)
        {
            string greeting = !string.IsNullOrEmpty(petName) ? $"for {petName} " : "";

            return $@"You are a world-class pet care expert specializing in the {breed} {petType.ToString()} breed.
Provide one practical, non-obvious, and helpful tip {greeting}based on their specific breed characteristics.

Requirements:
- The tip must be 1-2 sentences maximum.
- The tip must be actionable and specific to the {breed} breed (not just for {petType.ToString()}s in general).
- Focus on a specific aspect of health, nutrition, training, grooming, or exercise.

CRITICAL:
- DO NOT provide generic or common-sense advice (e.g., 'love your pet', 'give them fresh water', 'play with them', 'take them to the vet').
- DO NOT just state a fact; it must be an actionable *tip*.

Breed-Specific Tip:";
        }

        private (string url, StringContent content) BuildRequest(string prompt)
        {
            var request = new GeminiRequest
            {
                Contents = new List<GeminiContent>
            {
                new GeminiContent
                {
                    Parts = new List<GeminiPart>
                    {
                        new GeminiPart { Text = prompt }
                    }
                }
            },
                GenerationConfig = new GenerationConfig
                {
                    Temperature = 0.5,
                    MaxOutputTokens = 100
                }
            };

            var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.0-flash:generateContent?key={_apiKey}";
            var jsonContent = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            return(url, content);

        }
    }

    }

