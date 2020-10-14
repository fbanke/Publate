using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class PostFactoryTest
    {
        [Fact]
        public void Should_GivePublicAndPublishedTextPost()
        {
            var authorUrn = new Urn("urn:namespace:entityName:id");
            var factory = new PostFactory();

            var post = factory.CreatePublicPublishedTextPost(authorUrn, "post text");

            Assert.Equal(Visibility.Reach.Public, post.Visibility.State);
            Assert.Equal(ContentState.LifecycleState.Published, post.LifecycleState.State);
        }
    }
}