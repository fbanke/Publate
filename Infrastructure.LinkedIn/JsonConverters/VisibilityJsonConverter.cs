using System;
using Newtonsoft.Json;

namespace Infrastructure.LinkedIn.JsonConverters
{
    public class VisibilityJsonConverter : JsonConverter<Visibility>
    {
        public override void WriteJson(JsonWriter writer, Visibility value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(value.JsonName);
            
            serializer.Serialize(writer, value.ToString());
            
            writer.WriteEndObject();
        }

        public override Visibility ReadJson(JsonReader reader, Type objectType, Visibility existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Is never called as long as CanRead is false");
        }

        public override bool CanRead { get; } = false;
    }
}