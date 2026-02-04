using Newtonsoft.Json;

namespace Trados.GenAI.LMStudio.Models
{
    // Later on add image
    // For now let's see how it goes
    public class MessageItem
    {
        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("content")]
        public List<MessageItemPart> Content { get; set; } = new List<MessageItemPart>();

        public static MessageItem CreateSystemMessageItem(string content)
        {
            var messageItem = new MessageItem
            {
                Role = "system",
            };

            messageItem.Content.Add(MessageItemPart.CreateMessageTextItemPart(content));
            return messageItem;
        }

        public static MessageItem CreateUserMessageItem(string input)
        {
            var messageItem = new MessageItem
            {
                Role = "user"
            };

            if (!string.IsNullOrEmpty(input))
            {
                messageItem.Content.Add(MessageItemPart.CreateMessageTextItemPart(input));
            }

            return messageItem;
        }
    }
}
