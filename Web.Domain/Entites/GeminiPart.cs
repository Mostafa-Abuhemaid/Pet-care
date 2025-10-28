using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GeminiPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

}
