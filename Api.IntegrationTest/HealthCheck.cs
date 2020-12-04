using System.Threading.Tasks;
using Xunit; 

namespace Api.IntegrationTest
{
    public class HealthCheck
    {
        private const string Url = "/health";

        [Fact]
        public async Task Should_GiveHealthyResponse_WhenSystemIsHealthy()
        {
            CircuitBreakerHealthCheck.Healthy = true;
            
            using var webApplicationFactory = new IntegrationTestWebApplicationFactory();
            var response = await webApplicationFactory.CreateClient().GetAsync(Url);
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
        }
        
        [Fact]
        public async Task Should_GiveUnhealthyResponse_WhenSystemIsUnhealthy()
        {
            CircuitBreakerHealthCheck.Healthy = false;
            
            using var webApplicationFactory = new IntegrationTestWebApplicationFactory();
            var response = await webApplicationFactory.CreateClient().GetAsync(Url);
            
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal("Unhealthy", await response.Content.ReadAsStringAsync());
        }
    }
}