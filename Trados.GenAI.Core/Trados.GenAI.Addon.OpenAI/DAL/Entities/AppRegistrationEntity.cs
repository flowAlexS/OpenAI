using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Trados.GenAI.Addon.OpenAI.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class AppRegistrationEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The client credentials.
        /// </summary>
        public ClientCredentialsEntity ClientCredentials { get; set; }
    }
}
