using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GeminiContent
    {
        [JsonPropertyName("parts")]
        public List<GeminiPart> Parts { get; set; } = new();
    }

}
