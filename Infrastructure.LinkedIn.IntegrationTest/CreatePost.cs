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
        private readonly PostJsonSerializationService _serializationService;

        public CreatePost()
        {
            _linkedInSettings = LinkedInSettings.Create();
            _serializationService = new PostJsonSerializationService();
        }
        
        [Fact(Skip = "Can only be run manually"), Priority(-8)]
        public async Task Should_CreatePost_When_SubmittingValidPost()
        {
            const string postText = "This is a test post from the publate.com API project";
            var post = new Post(
                LinkedInSettings.GetMeId(),
                new ContentState(ContentState.LifecycleState.Published),
                new ShareContent(new Message(postText), new MediaType(MediaType.Type.None)),
                new Visibility(Visibility.Reach.Public)
            );
            
            var postCreated = await CreateLinkedInPost(post);
            Assert.NotEmpty(postCreated.id);
        }
        
        [Fact(Skip = "Can only be run manually"), Priority(-8)]
        public async Task Should_GiveThrowException_When_PostingToLargeMessage()
        {
            var toLongPostText = new string('*', _linkedInSettings.CharacterLimitOnPosts + 1);
            
            var post = new Post(
                LinkedInSettings.GetMeId(),
                new ContentState(ContentState.LifecycleState.Published),
                new ShareContent(new Message(toLongPostText), new MediaType(MediaType.Type.None)),
                new Visibility(Visibility.Reach.Public)
            );

            var json = _serializationService.Serialize(post);
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await CreateLinkedInPost(post));
            Assert.Contains("exceeded the maximum allowed", exception.Message);
        }
        
        private async Task<PostCreateResponse> CreateLinkedInPost(Post post)
        {
            var client = _linkedInSettings.CreateApiClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "/v2/ugcPosts")
            {
                Content = new StringContent(_serializationService.Serialize(post))
            };

            var response = await client.SendAsync(request);

            var postCreateJson = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Created)
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