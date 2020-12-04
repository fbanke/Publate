using System.Threading.Tasks;
using Xunit; 

namespace Api.IntegrationTest
{
    public class ApiSmokeTest
    {
        [Fact]
        public async Task TestThat_SwaggerApiDocumentationUrl_ReturnsSuccess()
        {
            using var webApplicationFactory = new IntegrationTestWebApplicationFactory();
            const string url = "/swagger/v1/swagger.json";
            var response = await webApplicationFactory.CreateClient().GetAsync(url);
            
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}