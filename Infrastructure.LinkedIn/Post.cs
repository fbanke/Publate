namespace Infrastructure.LinkedIn
{
    public class Post
    {
        public Urn Author { get; }
        public ContentState LifecycleState { get; }
        public ShareContent SpecificContent { get; }
        public Visibility Visibility { get; }

        public Post(Urn author, ContentState state, ShareContent shareContent, Visibility visibility)
        {
            Author = author;
            LifecycleState = state;
            SpecificContent = shareContent;
            Visibility = visibility;
        }
    }
}