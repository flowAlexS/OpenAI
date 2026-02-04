using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class CompletionResponseItem
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
