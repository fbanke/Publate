using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.IntegrationTest
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public const string TestScheme = "Test Scheme";
        
        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(),
                TestScheme);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, 
            Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(
                TestAuthenticationHandler.TestScheme, TestAuthenticationHandler.TestScheme, configureOptions
            );
        }
    }

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public static string UserId = Guid.NewGuid().ToString(); 
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new[]
        {
            new Claim(
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", 
                UserId
            ),
        }, "test");
    }
}