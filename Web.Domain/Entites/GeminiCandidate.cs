using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent? Content { get; set; }
    }

}
