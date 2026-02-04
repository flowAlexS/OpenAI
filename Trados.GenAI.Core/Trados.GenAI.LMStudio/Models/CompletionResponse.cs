using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class CompletionResponse
    {
        [JsonProperty("choices")]
        public List<CompletionResponseItem> Choices { get; set; } = new List<CompletionResponseItem>();
    }
}
