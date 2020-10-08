using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    public class LinkedInSettings
    {
        private const string AccessTokenFilePath = "accesstoken.txt";
        private const string MeIdFilePath = "meid.txt";
        public int CharacterLimitOnPosts = 1300;
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string CallbackUrl { get; set; }
        public string LinkedInAuthUrl { get; set; }

        public static LinkedInSettings Create()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<LinkedInSettings>();
            var configuration = builder.Build();
            
            var secrets = configuration.GetSection("LinkedIn").Get<LinkedInSettings>();
            
            if (string.IsNullOrEmpty(secrets.ClientId))
            {
                throw new ArgumentException("LinkedIn ClientId not set in user secrets");
            }
            
            if (string.IsNullOrEmpty(secrets.ClientSecret))
            {
                throw new ArgumentException("LinkedIn ClientSecret not set in user secrets");
            }
            
            if (string.IsNullOrEmpty(secrets.CallbackUrl))
            {
                throw new ArgumentException("LinkedIn Callback url not set in user secrets");
            }

            secrets.LinkedInAuthUrl = $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={secrets.ClientId}&redirect_uri={HttpUtility.UrlEncode(secrets.CallbackUrl)}&state=&scope=r_liteprofile%20r_emailaddress%20w_member_social";
            
            return secrets;
        }

        public async Task SaveAccessToken(string accessToken)
        {
            await File.WriteAllTextAsync(AccessTokenFilePath, accessToken);
        }
        
        public string GetAccessToken()
        {
            return File.ReadAllText(AccessTokenFilePath);
        }

        public static async Task SaveMeId(string meId)
        {
            await File.WriteAllTextAsync(MeIdFilePath, meId);
        }
        
        public HttpClient CreateApiClient()
        {
            var client = new HttpClient {BaseAddress = new Uri("https://api.linkedin.com")};
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetAccessToken());
            client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            
            return client;
        }

        public string GetMeId()
        {
            return File.ReadAllText(MeIdFilePath);
        }
    }
}