using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class GetMe
    {
        private readonly LinkedInSettings _linkedInSettings;

        public GetMe()
        {
            _linkedInSettings = LinkedInSettings.Create();
        }
        
        [Fact, Priority(-9)]
        public async Task Should_GetLinkedInMeObject_When_AccessTokenIsValid()
        {
            var me = await RequestMeDataFromLinkedIn();
            
            Assert.NotEmpty(me.id);
            await LinkedInSettings.SaveMeId("urn:li:person:" + me.id);
        }
        
        private async Task<Me> RequestMeDataFromLinkedIn()
        {
            var client = _linkedInSettings.CreateApiClient();
            var response = await client.GetAsync("/v2/me");
            
            var meJson = await response.Content.ReadAsStringAsync();
            var me = JsonConvert.DeserializeObject<Me>(meJson);
            
            return me;
        }
    }
    
    public class ProfilePicture    {
        public string displayImage { get; set; } 
    }

    public class Me
    {
        public string id { get; set; }

        public ProfilePicture profilePicture { get; set; } 
    }
}