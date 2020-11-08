using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Auth0
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
    }
}