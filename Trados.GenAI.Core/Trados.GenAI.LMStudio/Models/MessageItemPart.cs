using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    public class MessageItemPart
    {
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string? Text { get; set; }

        /// <summary>
        /// Factory method to create a text MessageItemPart
        /// </summary>
        public static MessageItemPart CreateMessageTextItemPart(string text)
        {
            return new MessageItemPart
            {
                Type = "text",
                Text = text
            };
        }

        public static MessageItemPart CreateMessageImageItemPart(string base64)
        {
            return new MessageItemImagePart
            {
                Type = "image",
                ImageUri = new ImageUriModel() { Url = base64 },    
            };
        }
    }
}
