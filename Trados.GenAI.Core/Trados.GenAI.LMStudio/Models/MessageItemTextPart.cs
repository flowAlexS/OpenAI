using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    internal class MessageItemTextPart : MessageItemPart
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
