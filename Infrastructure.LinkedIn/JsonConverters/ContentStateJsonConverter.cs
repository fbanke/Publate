using System;
using Newtonsoft.Json;

namespace Infrastructure.LinkedIn.JsonConverters
{
    public class ContentStateJsonConverter : JsonConverter<ContentState>
    {
        public override void WriteJson(JsonWriter writer, ContentState value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override ContentState ReadJson(JsonReader reader, Type objectType, ContentState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Is never called as long as CanRead is false");
        }

        public override bool CanRead { get; } = false;
    }
}