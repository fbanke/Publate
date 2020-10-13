using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class PostTest
    {
        [Fact]
        public void Should_SerializeJsonAccordingToSpec_When_GivenPost()
        {
            const string authorUrn = "urn:namespace:entityName:id";
            const string postText = "post text";
            
            var author = new Urn(authorUrn);
            var state = new ContentState(ContentState.LifecycleState.Published);
            var mediaCategory = new MediaType(MediaType.Type.None);
            var visibility = new Visibility(Visibility.Reach.Public);
            
            var post = new Post(author, state, new ShareContent(new Message(postText), mediaCategory),visibility);

            var serializer = new PostJsonSerializationService();
            
            Assert.Equal(
                "{" +
                    "\"author\":\""+authorUrn+"\"," +
                    "\"lifecycleState\":\""+state+"\"," +
                    "\"specificContent\":{" +
                        "\"com.linkedin.ugc.ShareContent\":{" +
                            "\"shareCommentary\":{\"text\":\""+postText+"\"}," +
                            "\"shareMediaCategory\":\""+mediaCategory+"\"" +
                        "}" +
                    "}," +
                    "\"visibility\":{" +
                        "\"com.linkedin.ugc.MemberNetworkVisibility\":\""+visibility+"\"" +
                    "}" +
                "}", 
                serializer.Serialize(post));
        }
    }
}