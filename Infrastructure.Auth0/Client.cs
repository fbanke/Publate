using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Infrastructure.Auth0
{
    public class Client
    {
        private readonly Settings _settings;

        public Client(Settings settings)
        {
            _settings = settings;
        }
        public HttpClient Create()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri($"https://{_settings.Domain}")
            };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

            return client;
        }
    }
}