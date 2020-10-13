using System;
using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class VisibilityJsonConverterTest
    {
        [Fact]
        public void Should_SerializeToVisibility_When_GivenValidVisibility()
        {
            const string visibilityString = "Public";
            var visibility = new Visibility((Visibility.Reach)Enum.Parse(typeof(Visibility.Reach), visibilityString));
            var visibilityJson = JsonConvert.SerializeObject(visibility, new VisibilityJsonConverter());
            
            Assert.Equal("{\"com.linkedin.ugc.MemberNetworkVisibility\":\"PUBLIC\"}", visibilityJson);
        }
    }
}