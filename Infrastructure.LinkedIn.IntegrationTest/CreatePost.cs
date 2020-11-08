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
        private readonly Settings _settings;
        private readonly PostJsonSerializationService _serializationService;
        private readonly PostFactory _factory;
        private readonly TokenRepository _tokenRepository;

        public CreatePost()
        {
            var settingsFactory = new SettingsFactory();
            _tokenRepository = new TokenRepository();
            _settings = settingsFactory.Create();

            _serializationService = new PostJsonSerializationService();
            _factory = new PostFactory();
        }
        
        [Fact(Skip = "Can only be run manually"), Priority(-8)]
        public async Task Should_CreatePost_When_SubmittingValidPost()
        {
            const string postText = "This is a test post from the publate.com API project";
            var post = _factory.CreatePublicPublishedTextPost(_tokenRepository.GetMeId(), postText);
            
            var postCreated = await CreateLinkedInPost(post);
            Assert.NotEmpty(postCreated.id);
        }
        
        [Fact(Skip = "Can only be run manually"), Priority(-8)]
        public async Task Should_GiveThrowException_When_PostingToLargeMessage()
        {
            var toLongPostText = new string('*', _settings.CharacterLimitOnPosts + 1);
            
            var post = _factory.CreatePublicPublishedTextPost(_tokenRepository.GetMeId(), toLongPostText);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await CreateLinkedInPost(post));
            Assert.Contains("exceeded the maximum allowed", exception.Message);
        }
        
        private async Task<PostCreateResponse> CreateLinkedInPost(Post post)
        {
            var client = _tokenRepository.CreateApiClient();
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