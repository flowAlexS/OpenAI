using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Trados.GenAI.Addon.OpenAI.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ModelType
    {
        [EnumMember(Value = "Completion")]
        Completion,

        [EnumMember(Value = "Chat\\Completion")]
        ChatCompletion
    }
}
