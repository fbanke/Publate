namespace Infrastructure.LinkedIn
{
    public class ShareContent
    {
        public readonly string JsonName = "com.linkedin.ugc.ShareContent";

        public Message ShareCommentary { get; }
        public MediaType ShareMediaCategory { get; }

        public ShareContent(Message message, MediaType mediaType)
        {
            ShareCommentary = message;
            ShareMediaCategory = mediaType;
        }
    }
}