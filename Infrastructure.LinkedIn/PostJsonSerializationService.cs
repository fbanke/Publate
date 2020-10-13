using System.Collections.Generic;
using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.LinkedIn
{
    public class PostJsonSerializationService
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public PostJsonSerializationService()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new UrnJsonConverter(), 
                    new ContentStateJsonConverter(),
                    new ShareContentJsonConverter(),
                    new MediaTypeJsonConverter(),
                    new VisibilityJsonConverter(),
                }
            };
        }

        public string Serialize(Post post)
        {
            return JsonConvert.SerializeObject(post, _serializerSettings);
        }
    }
}