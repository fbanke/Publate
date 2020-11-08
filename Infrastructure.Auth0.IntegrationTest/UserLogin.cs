using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.Auth0.IntegrationTest 
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserLogin : IClassFixture<SettingsFixture>
    {
        private readonly Settings _settings;
        
        public UserLogin(SettingsFixture settings)
        {
            _settings = settings.Settings;
        }
        
        [Fact(Skip = "Can only be run manually, interactive test")]
        public void Should_GetOauthIdToken_When_UserLoginInteractively()
        {
            var nonce = GenerateNonce();
            var interactiveUserLoginWaiter = new AutoResetEvent(false);
            var oAuthIdTokenStore = new OAuthIdTokenStore(interactiveUserLoginWaiter);

            var webServerCancellationToken = StartWebServerForCallbackRequestInBackground(oAuthIdTokenStore);

            OpenUrlInBrowser(_settings.AuthUrl(nonce));

            interactiveUserLoginWaiter.WaitOne();
            
            Assert.NotEmpty(oAuthIdTokenStore.IdToken);
            
            var idToken = ParseIdToken(oAuthIdTokenStore);

            Assert.Equal(nonce, GetClaimsValue(idToken, "nonce"));
            Assert.Single(idToken.Audiences);
            Assert.Equal(_settings.ClientId, idToken.Audiences.First());
            
            webServerCancellationToken.Cancel();
        }

        private static JwtSecurityToken ParseIdToken(OAuthIdTokenStore oAuthIdTokenStore)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadToken(oAuthIdTokenStore.IdToken) as JwtSecurityToken;
            return jwt;
        }

        private static string GenerateNonce()
        {
            var random = new Random();
            var nonce = random.Next(123400, 9999999).ToString();
            return nonce;
        }

        private static string GetClaimsValue(JwtSecurityToken jwt, string claim)
        {
            return jwt.Claims.First(x => x.Type == claim).Value;
        }

        private static void OpenUrlInBrowser(string url)
        {
            url = EscapeUrlForWindowsCmd(url);
            System.Diagnostics.Process.Start("cmd", "/C start " + url);
        }

        private static string EscapeUrlForWindowsCmd(string url)
        {
            url = url.Replace("&", "^&");
            return url;
        }

        private static CancellationTokenSource StartWebServerForCallbackRequestInBackground(OAuthIdTokenStore idTokenStore)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            
            Task.Run(() =>
            {
                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<IStartup>(new Startup(idTokenStore));
                    })
                    .Build();
                host.StartAsync(token);
            }, token);

            return tokenSource;
        }
    }
    
    public class OAuthIdTokenStore
    {
        public string IdToken;
        public readonly AutoResetEvent AutoResetEvent;

        public OAuthIdTokenStore(AutoResetEvent autoResetEvent)
        {
            AutoResetEvent = autoResetEvent;
        }
    }
    public class Startup : IStartup
    {
        private readonly OAuthIdTokenStore _idTokenStore;

        public Startup(OAuthIdTokenStore idTokenStore)
        {
            _idTokenStore = idTokenStore;
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var idToken = context.Request.Query["id_token"];
                if (idToken.Count > 0)
                {
                    _idTokenStore.IdToken = idToken;
                    _idTokenStore.AutoResetEvent.Set();
                }

                var javascriptToReturnIdTokenToServer = "<script>" +
                             "var id_token = window.location.hash.substr(\"#id_token=\".length);" +
                             "var http_requester = new XMLHttpRequest();" +
                             "http_requester.open( \"GET\", \"/?id_token=\"+id_token );" +
                             "http_requester.send( null );" +
                             "</script>";
                await context.Response.WriteAsync(
                    javascriptToReturnIdTokenToServer);
            });
        }
    }
}
