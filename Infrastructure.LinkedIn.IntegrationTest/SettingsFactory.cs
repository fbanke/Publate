using System;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.LinkedIn.IntegrationTest
{
    public class SettingsFactory
    {
        public Settings Create()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets<Settings>();
            var configuration = builder.Build();
            
            var secrets = configuration.GetSection("LinkedIn").Get<Settings>();
            
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

            secrets.LinkedInAuthUrl = $"https://www.linkedin.com/oauth/v2/authorization?"+
                                      "response_type=code&"+
                                      $"client_id={secrets.ClientId}&"+
                                      $"redirect_uri={HttpUtility.UrlEncode(secrets.CallbackUrl)}&"+
                                      "state=&"+
                                      "scope=r_liteprofile%20r_emailaddress%20w_member_social";
            
            return secrets;
        }
    }
}