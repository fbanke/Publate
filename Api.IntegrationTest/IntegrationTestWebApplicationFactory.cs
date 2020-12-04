using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.IntegrationTest
{
    public class IntegrationTestWebApplicationFactory : WebApplicationFactory<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder => builder.UseStartup<TestStartup>());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {   
            builder.UseEnvironment("IntegrationTest");
        }
    }

    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration){}

        protected override void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthenticationHandler.TestScheme; 
                options.DefaultChallengeScheme = TestAuthenticationHandler.TestScheme;
            }).AddTestAuth(o => { });
        }
    }
}