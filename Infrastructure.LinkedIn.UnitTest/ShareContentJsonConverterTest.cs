using System.Collections.Generic;
using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class ShareContentJsonConverterTest
    {
        [Fact]
        public void Should_Serialize_When_GivenValid()
        {
            const string postText = "post text";
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new ShareContentJsonConverter(),
                    new MediaTypeJsonConverter()
                }
            };
            
            var shareContent = new ShareContent(new Message(postText), new MediaType(MediaType.Type.None));
            var shareContentJson = JsonConvert.SerializeObject(shareContent, serializerSettings);
            
            Assert.Equal("{\"com.linkedin.ugc.ShareContent\":{\"shareCommentary\":{\"text\":\""+postText+"\"},\"shareMediaCategory\":\"NONE\"}}", shareContentJson);
        }
    }
}