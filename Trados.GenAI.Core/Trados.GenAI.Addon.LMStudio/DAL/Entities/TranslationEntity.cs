using MongoDB.Bson.Serialization.Attributes;

namespace Trados.GenAI.Addon.LMStudio.DAL.Entities
{
    [BsonIgnoreExtraElements]
    public class TranslationEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// AI Model used for request
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The tenant id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// The system instructions used for translation
        /// </summary>
        public string SystemInstructions { get; set; }

        /// <summary>
        /// The user prompt used for translation
        /// </summary>
        public string UserPrompt { get; set; }

        /// <summary>
        /// Context Image uri used for translation
        /// </summary>
        public string ContextImage { get; set; }

        /// <summary>
        /// Source Segment as text
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Target Segment as text
        /// </summary>
        public string TargetText { get; set; }
    }

}
