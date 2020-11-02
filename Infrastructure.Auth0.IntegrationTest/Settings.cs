using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Auth0.IntegrationTest
{
    public class Settings
    {
        public string AccessToken { get; set; }
        public string ManagementApiClientId { get; set; }
        public string ManagementApiClientSecret { get; set; }
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string CallbackUrl = "https://local.publate.com:5001/auth/linkedin/callback";

        public string AuthUrl(string nonce) =>  
            $"https://{Domain}/authorize"+
            $"?client_id={ClientId}"+
            "&response_type=id_token"+
            "&connection=linkedin"+
            "&prompt=login"+
            "&scope=openid%20profile"+
            "&connection_scope=w_member_social"+
            $"&nonce={nonce}"+
            $"&redirect_uri={HttpUtility.UrlEncode(CallbackUrl)}";

        public static Settings Create()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Settings>();
            builder.AddEnvironmentVariables();
            
            var configuration = builder.Build();

            var secrets = configuration.GetSection("Auth0").Get<Settings>();

            if (string.IsNullOrEmpty(secrets.ManagementApiClientId))
            {
                throw new ArgumentException("Auth0 Management API ClientId not set in user secrets");
            }

            if (string.IsNullOrEmpty(secrets.ManagementApiClientSecret))
            {
                throw new ArgumentException("Auth0 Management API ClientSecret not set in user secrets");
            }
            
            if (string.IsNullOrEmpty(secrets.Domain))
            {
                throw new ArgumentException("Auth0 Domain not set in user secrets");
            }

            return secrets;
        }

        public HttpClient CreateApiClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://{Domain}")
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AccessToken);

            return client;
        }
    }
}