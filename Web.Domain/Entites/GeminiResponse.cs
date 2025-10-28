using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate>? Candidates { get; set; }
    }

}
