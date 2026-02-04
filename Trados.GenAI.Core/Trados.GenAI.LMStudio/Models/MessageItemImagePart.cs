using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class MessageItemImagePart : MessageItemPart
    {
        [JsonProperty("image_url")]
        public ImageUriModel? ImageUri { get; set; } 
    }
}
