using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class ChatCompletionRequest
    {
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("messages")]
        public List<MessageItem> Messages { get; set; } = new List<MessageItem>();

        [JsonProperty("temperature")]
        public float Temperature { get; set; } = 0.7f;
    }
}
