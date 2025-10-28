using System.Text.Json.Serialization;

namespace Web.Domain.Entites
{
    public class GenerationConfig
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.8;

        [JsonPropertyName("maxOutputTokens")]
        public int MaxOutputTokens { get; set; } = 200;
    }

}
