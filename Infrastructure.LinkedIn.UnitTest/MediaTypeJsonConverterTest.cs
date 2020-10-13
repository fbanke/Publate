using System;
using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class MediaTypeJsonConverterTest
    {
        [Fact]
        public void Should_SerializeToMediaType_When_GivenValidMediaType()
        {
            const string mediaTypeString = "None";
            var mediaType = new MediaType((MediaType.Type)Enum.Parse(typeof(MediaType.Type), mediaTypeString));
            var mediaTypeJson = JsonConvert.SerializeObject(mediaType, new MediaTypeJsonConverter());
            
            Assert.Equal($"\"NONE\"", mediaTypeJson);
        }
    }
}