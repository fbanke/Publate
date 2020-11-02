namespace Infrastructure.Auth0.IntegrationTest
{
    public class SettingsFixture
    {
        public readonly Settings Settings;
        
        public SettingsFixture()
        {
            Settings = Settings.Create();
        }
    }
}