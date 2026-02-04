using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Nodes;

namespace Trados.GenAI.Addon.LMStudio.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class ConfigurationValueEntity
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The value object.
        /// </summary>
        public JsonNode Value { get; set; }
    }
}
