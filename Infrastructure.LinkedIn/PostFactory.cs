namespace Infrastructure.LinkedIn
{
    public class PostFactory
    {
        public Post CreatePublicPublishedTextPost(Urn me, string text)
        {
            return new Post(
                me,
                new ContentState(ContentState.LifecycleState.Published),
                new ShareContent(new Message(text), new MediaType(MediaType.Type.None)),
                new Visibility(Visibility.Reach.Public)
            );
        }
    }
}