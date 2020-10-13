using System;
using Newtonsoft.Json;

namespace Infrastructure.LinkedIn.JsonConverters
{
    public class ShareContentJsonConverter : JsonConverter<ShareContent>
    {
        public override void WriteJson(JsonWriter writer, ShareContent value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(value.JsonName);
            
            writer.WriteStartObject();
            writer.WritePropertyName("shareCommentary");
            serializer.Serialize(writer, value.ShareCommentary);
            
            writer.WritePropertyName("shareMediaCategory");
            serializer.Serialize(writer, value.ShareMediaCategory);
            writer.WriteEndObject();
            
            writer.WriteEndObject();
        }

        public override ShareContent ReadJson(JsonReader reader, Type objectType, ShareContent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Is never called as long as CanRead is false");
        }

        public override bool CanRead { get; } = false;
    }
}