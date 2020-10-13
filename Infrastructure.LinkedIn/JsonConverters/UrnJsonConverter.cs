using System;
using Newtonsoft.Json;

namespace Infrastructure.LinkedIn.JsonConverters
{
    public class UrnJsonConverter : JsonConverter<Urn>
    {
        public override void WriteJson(JsonWriter writer, Urn value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override Urn ReadJson(JsonReader reader, Type objectType, Urn existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var s = (string)reader.Value;
            return new Urn(s);
        }
    }
}