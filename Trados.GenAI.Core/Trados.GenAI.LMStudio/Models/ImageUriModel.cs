using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class ImageUriModel
    {
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;
    }
}
