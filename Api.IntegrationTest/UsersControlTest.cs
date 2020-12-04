using System.Threading.Tasks;
using Xunit; 

namespace Api.IntegrationTest
{
    public class UsersControlTest
    {
        [Fact]
        public async Task Should_GivenMeUser_GiveSuccessAndSettings()
        {
            using var webApplicationFactory = new IntegrationTestWebApplicationFactory();
            
            const string userId = "me";

            TestAuthenticationOptions.UserId = userId;
            
            const string expected = @"{""userId"":""me""}";

            var url = $"/users/{userId}/settings";
            var response = await webApplicationFactory.CreateDefaultClient().GetAsync(url);

            Assert.True(response.IsSuccessStatusCode, $"Status code was: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(expected, content);
        }
    }
}