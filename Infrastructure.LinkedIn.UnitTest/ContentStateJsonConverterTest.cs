using System;
using Infrastructure.LinkedIn.JsonConverters;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class ContentStateJsonConverterTest
    {
        [Fact]
        public void Should_SerializeToContentState_When_GivenValidContentState()
        {
            const string lifeCycleStateString = "Published";
            var contentState = new ContentState((ContentState.LifecycleState)Enum.Parse(typeof(ContentState.LifecycleState), lifeCycleStateString));
            var contentStateJson = JsonConvert.SerializeObject(contentState, new ContentStateJsonConverter());
            
            Assert.Equal($"\"PUBLISHED\"", contentStateJson);
        }
    }
}