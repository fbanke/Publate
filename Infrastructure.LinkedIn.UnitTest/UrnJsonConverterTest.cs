using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class UrnJsonConverterTest
    {
        [Fact]
        public void Should_SerializeToUrn_When_GivenValidUrn()
        {
            const string urnString = "urn:namespace:entityName:id";
            var urn = new Urn(urnString);
            var urnJson = JsonConvert.SerializeObject(urn, new UrnJsonConverter());
            
            Assert.Equal($"\"{urnString}\"", urnJson);
        }
        
        [Fact]
        public void Should_DeserializeToUrn_When_GivenValidUrn()
        {
            const string urnString = "urn:namespace:entityName:id";
            var urnJson = $"\"{urnString}\"";
            var urn = JsonConvert.DeserializeObject<Urn>(urnJson, new UrnJsonConverter());
            
            Assert.Equal(urnString, urn.ToString());
        }
    }
}