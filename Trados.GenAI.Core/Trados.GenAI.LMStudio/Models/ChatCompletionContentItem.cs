using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class ChatCompletionContentItem
    {
        [JsonProperty("message")]
        public ChatCompletionContentMessage Message { get; set; } = new();
    }
}
