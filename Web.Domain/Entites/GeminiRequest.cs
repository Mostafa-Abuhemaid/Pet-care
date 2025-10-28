using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GeminiRequest
    {
        [JsonPropertyName("contents")]
        public List<GeminiContent> Contents { get; set; } = new();

        [JsonPropertyName("generationConfig")]
        public GenerationConfig? GenerationConfig { get; set; }
    }

}
