using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Priority;

namespace Infrastructure.Auth0.IntegrationTest 
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ManagementApi : IClassFixture<SettingsFixture>
    {
        private readonly Settings _settings;
        private readonly Client _client;
        
        public ManagementApi(SettingsFixture settings)
        {
            _settings = settings.Settings;
            _client = new Client(_settings);
        }
        
        [Fact, Priority(1)]
        public async void Should_GetAccessTokenForManagementApi_When_CredentialsAreValid()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://{_settings.Domain}")
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");

            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _settings.ManagementApiClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ManagementApiClientSecret),
                new KeyValuePair<string, string>("audience", $"https://{_settings.Domain}/api/v2/")
            };

            request.Content = new FormUrlEncodedContent(formData);

            var response = await client.SendAsync(request);

            var auth0Json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Auth0AccessToken>(auth0Json);
      
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotEmpty(token.access_token);

            _settings.AccessToken = token.access_token;
        }

        [Fact, Priority(2)]
        public async Task Should_FindUserProfile_When_GivenValidAccessTokenAndUserId()
        {
            const string userId = "linkedin%7CbPFi4OB_eh";
            var client = _client.Create();
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/users/{userId}");

            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Auth0User>(json);

            Assert.NotEmpty(user.name);
            Assert.Single(user.identities);
            var identity = user.identities[0];
            Assert.NotEmpty(identity.access_token);
            Assert.Equal("linkedin", identity.provider);

        }
    }
}

public class Auth0User
{
    public string name { get; set; }
    public List<Auth0Identity> identities { get; set; } = new List<Auth0Identity>();
}

public class Auth0Identity
{
    public string access_token { get; set; }
    public string provider { get; set; }
}

public class Auth0AccessToken
{
    public string access_token { get; set; } 
    public int expires_in { get; set; } 
}