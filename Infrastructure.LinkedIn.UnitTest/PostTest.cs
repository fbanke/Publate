using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class PostTest
    {
        private readonly PostFactory _factory;

        public PostTest()
        {
            _factory = new PostFactory();
        }
        [Fact]
        public void Should_SerializeJsonAccordingToSpec_When_GivenPost()
        {
            const string authorUrn = "urn:namespace:entityName:id";
            const string postText = "post text";
            
            var author = new Urn(authorUrn);

            var post = _factory.CreatePublicPublishedTextPost(author, postText);

            var serializer = new PostJsonSerializationService();
            
            Assert.Equal(
                "{" +
                    "\"author\":\""+authorUrn+"\"," +
                    "\"lifecycleState\":\""+post.LifecycleState+"\"," +
                    "\"specificContent\":{" +
                        "\"com.linkedin.ugc.ShareContent\":{" +
                            "\"shareCommentary\":{\"text\":\""+postText+"\"}," +
                            "\"shareMediaCategory\":\""+post.SpecificContent.ShareMediaCategory+"\"" +
                        "}" +
                    "}," +
                    "\"visibility\":{" +
                        "\"com.linkedin.ugc.MemberNetworkVisibility\":\""+post.Visibility+"\"" +
                    "}" +
                "}", 
                serializer.Serialize(post));
        }
    }
}