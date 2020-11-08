namespace Infrastructure.LinkedIn
{
    public class Settings
    {
        public readonly int CharacterLimitOnPosts = 1300;
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string CallbackUrl { get; set; }
        public string LinkedInAuthUrl { get; set; }
        
    }
}