using System;
using Newtonsoft.Json;

namespace Infrastructure.LinkedIn.JsonConverters
{
    public class MediaTypeJsonConverter : JsonConverter<MediaType>
    {
        public override void WriteJson(JsonWriter writer, MediaType value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override MediaType ReadJson(JsonReader reader, Type objectType, MediaType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Is never called as long as CanRead is false");
        }

        public override bool CanRead { get; } = false;
    }
}