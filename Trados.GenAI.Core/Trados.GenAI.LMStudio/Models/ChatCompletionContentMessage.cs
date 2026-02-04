using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class ChatCompletionContentMessage
    {
        [JsonProperty("content")]
        public string ContentMessage { get; set; } = string.Empty;
    }
}