using MongoDB.Bson.Serialization.Attributes;

namespace Trados.GenAI.Addon.OpenAI.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class ClientCredentialsEntity
    {
        /// <summary>
        /// The client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
