using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class ChatCompletionResponse
    {
        [JsonProperty("choices")]
        public List<ChatCompletionContentItem> Choices { get; set; } = new List<ChatCompletionContentItem>();
    }
}
