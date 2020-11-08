using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    public class TokenRepository
    {
        private const string AccessTokenFilePath = "accesstoken.txt";
        private const string MeIdFilePath = "meid.txt";

        public async Task SaveAccessToken(string accessToken)
        {
            await File.WriteAllTextAsync(AccessTokenFilePath, accessToken);
        }
        
        public string GetAccessToken()
        {
            return File.ReadAllText(AccessTokenFilePath);
        }

        public async Task SaveMeId(Urn meId)
        {
            await File.WriteAllTextAsync(MeIdFilePath, meId.ToString());
        }
        
        public HttpClient CreateApiClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.linkedin.com")
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", GetAccessToken());
            client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            
            return client;
        }

        public Urn GetMeId()
        {
            return new Urn(File.ReadAllText(MeIdFilePath)); 
        }
    }
}