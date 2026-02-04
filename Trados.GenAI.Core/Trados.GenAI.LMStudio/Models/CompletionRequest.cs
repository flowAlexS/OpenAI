using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class CompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("prompt")]
        public string Prompt {  get; set; } = string.Empty;

        [JsonProperty("temperature")]
        public float Temperature { get; set; } = 0.7f;

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = -1;
    }
}
