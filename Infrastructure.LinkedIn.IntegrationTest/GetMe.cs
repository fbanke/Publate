using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class GetMe
    {
        private readonly TokenRepository _tokenRepository;

        public GetMe()
        {
            _tokenRepository = new TokenRepository();
        }
        
        [Fact(Skip = "Can only be run manually"), Priority(-9)]
        public async Task Should_GetLinkedInMeObject_When_AccessTokenIsValid()
        {
            var me = await RequestMeDataFromLinkedIn();
            
            Assert.NotEmpty(me.id);
            var meId = new Urn("li", "person", me.id);
            await _tokenRepository.SaveMeId(meId);
        }
        
        private async Task<Me> RequestMeDataFromLinkedIn()
        {
            var client = _tokenRepository.CreateApiClient();
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