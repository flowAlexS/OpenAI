using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Trados.GenAI.Addon.LMStudio.DAL
{
    public class JsonNodeCustomSerializer : SerializerBase<JsonNode>
    {
        public override JsonNode Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            string stringValue = BsonSerializer.Deserialize<string>(context.Reader);

            try
            {
                return JsonNode.Parse(stringValue);
            }
            catch (Exception)
            {
                string stringJson = JsonSerializer.Serialize(stringValue);
                return JsonNode.Parse(stringJson);
            }
        }
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JsonNode value)
        {
            string stringValue = value?.ToJsonString();
            BsonSerializer.Serialize(context.Writer, stringValue);
        }
    }
}
