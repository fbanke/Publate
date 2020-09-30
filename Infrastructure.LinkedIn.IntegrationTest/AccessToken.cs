using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AccessToken
    {
        private readonly LinkedInSettings _linkedInSettings;

        public AccessToken()
        {
            _linkedInSettings = LinkedInSettings.Create();
        }
        
        [Fact, Priority(-10)]
        public async Task Should_GetLinkedInOauthAccessToken_When_UserLoginInteractively()
        {
            var userLoginWaiter = new AutoResetEvent(false);
            var oAuthCodeStore = new OAuthCodeStore(userLoginWaiter);

            var webServerCancellationToken = StartWebServerForCallbackRequestInBackground(oAuthCodeStore);

            OpenUrlInBrowser(_linkedInSettings.LinkedInAuthUrl);

            userLoginWaiter.WaitOne();
            
            var accessToken = await ExchangeOauthCodeForAccessToken(oAuthCodeStore.Code);

            Assert.NotEmpty(accessToken);
            
            await _linkedInSettings.SaveAccessToken(accessToken);
            
            webServerCancellationToken.Cancel();
        }

        private async Task<string> ExchangeOauthCodeForAccessToken(string linkedInOauthCode)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://www.linkedin.com")
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/v2/accessToken");

            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", linkedInOauthCode),
                new KeyValuePair<string, string>("redirect_uri", _linkedInSettings.CallbackUrl),
                new KeyValuePair<string, string>("client_id", _linkedInSettings.ClientId),
                new KeyValuePair<string, string>("client_secret", _linkedInSettings.ClientSecret)
            };

            request.Content = new FormUrlEncodedContent(formData);
            
            var response = await client.SendAsync(request);

            var linkedInJsonToken = await response.Content.ReadAsStringAsync();
            var linkedInToken = JsonConvert.DeserializeObject<LinkedInToken>(linkedInJsonToken);

            return linkedInToken.access_token;
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

        private static CancellationTokenSource StartWebServerForCallbackRequestInBackground(OAuthCodeStore codeStore)
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
                        services.AddSingleton<IStartup>(new Startup(codeStore));
                    })
                    .Build();
                host.StartAsync(token);
            }, token);

            return tokenSource;
        }
    }

    public class LinkedInToken
    {
        public string access_token { get; set; } 
        public int expires_in { get; set; } 
    }

    public class OAuthCodeStore
    {
        public string Code;
        public readonly AutoResetEvent AutoResetEvent;

        public OAuthCodeStore(AutoResetEvent autoResetEvent)
        {
            AutoResetEvent = autoResetEvent;
        }
    }
    public class Startup : IStartup
    {
        private readonly OAuthCodeStore _codeStore;

        public Startup(OAuthCodeStore codeStore)
        {
            _codeStore = codeStore;
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var code = context.Request.Query["code"];
                _codeStore.Code = code;
                _codeStore.AutoResetEvent.Set();

                await context.Response.WriteAsync("<p>Thanks, your LinkedIn oAuth code has been passed on. You can close this window</p>");
            });
        }
    }
}