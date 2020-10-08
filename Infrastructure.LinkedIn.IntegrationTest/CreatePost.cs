using System;
using System.Net;
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
        public async Task Should_CreatePost_When_SubmittingValidPost()
        {
            var postCreated = await CreateLinkedInPost("This is a test post from the publate.com API project");
            Assert.NotEmpty(postCreated.id);
        }
        
        [Fact, Priority(-8)]
        public async Task Should_GiveThrowException_When_PostingToLargeMessage()
        {
            var toLongPostText = new string('*', _linkedInSettings.CharacterLimitOnPosts + 1);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await CreateLinkedInPost(toLongPostText));
            Assert.Contains("exceeded the maximum allowed", exception.Message);
        }
        
        private async Task<PostCreateResponse> CreateLinkedInPost(string postText)
        {
            var client = _linkedInSettings.CreateApiClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/v2/ugcPosts")
            {
                Content = new StringContent(
                    "{\"author\": \"" + _linkedInSettings.GetMeId() +
                    "\",\"lifecycleState\": \"PUBLISHED\",\"specificContent\": {\"com.linkedin.ugc.ShareContent\": {\"shareCommentary\": {\"text\": \"" +
                    postText +
                    "\"},\"shareMediaCategory\": \"NONE\"}},\"visibility\": {\"com.linkedin.ugc.MemberNetworkVisibility\": \"PUBLIC\"}}")
            };

            var response = await client.SendAsync(request);

            var postCreateJson = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var postCreated = JsonConvert.DeserializeObject<PostCreateResponse>(postCreateJson);
                return postCreated;
            }
            var message = JsonConvert.DeserializeObject<ErrorResponse>(postCreateJson);
            throw new ArgumentException(message.message);
        }
    }
    
    public class PostCreateResponse
    {
        public string id { get; set; }
    }
    
    public class ErrorResponse
    {
        public string message { get; set; }
    }
}