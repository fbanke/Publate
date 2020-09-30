using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class CreatePost
    {
        private readonly LinkedInSettings _linkedInSettings;

        public CreatePost()
        {
            _linkedInSettings = LinkedInSettings.Create();
        }
        
        [Fact, Priority(-8)]
        public async Task Should_GetLinkedInOauthAccessToken_When_UserLoginInteractively()
        {
            var postCreated = await CreateLinkedInPost("This is a test post from by newest API project");
            
            Assert.NotEmpty(postCreated.id);
        }
        
        private async Task<PostCreateResponse> CreateLinkedInPost(string postText)
        {
            var client = _linkedInSettings.CreateApiClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/v2/ugcPosts");

            request.Content = new StringContent(
                "{\"author\": \""+_linkedInSettings.GetMeId()+"\",\"lifecycleState\": \"PUBLISHED\",\"specificContent\": {\"com.linkedin.ugc.ShareContent\": {\"shareCommentary\": {\"text\": \""+postText+"\"},\"shareMediaCategory\": \"NONE\"}},\"visibility\": {\"com.linkedin.ugc.MemberNetworkVisibility\": \"PUBLIC\"}}");
            
            var response = await client.SendAsync(request);

            var postCreateJson = await response.Content.ReadAsStringAsync();
            var postCreated = JsonConvert.DeserializeObject<PostCreateResponse>(postCreateJson);

            return postCreated;
        }
    }
    
    public class PostCreateResponse
    {
        public string id { get; set; }
    }
}