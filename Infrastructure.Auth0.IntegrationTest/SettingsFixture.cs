namespace Infrastructure.Auth0.IntegrationTest
{
    public class SettingsFixture
    {
        public readonly Settings Settings;
        
        public SettingsFixture()
        {
            var factory = new SettingsFactory();
            Settings = factory.Create();
        }
    }
}