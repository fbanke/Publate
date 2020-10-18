using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {   
            builder.UseEnvironment("IntegrationTest");
            
            builder.ConfigureServices(services =>
            {
               
            });
        }
    }
}