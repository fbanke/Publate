using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Auth0.IntegrationTest
{
    public class SettingsFactory
    {
        public Settings Create()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets(typeof(SettingsFactory).GetTypeInfo().Assembly); 
            builder.AddEnvironmentVariables();
            
            var configuration = builder.Build();

            var secrets = configuration.GetSection("Auth0").Get<Settings>();

            if (string.IsNullOrEmpty(secrets.ManagementApiClientId))
            {
                throw new ArgumentException("Auth0 Management API ClientId not set");
            }

            if (string.IsNullOrEmpty(secrets.ManagementApiClientSecret))
            {
                throw new ArgumentException("Auth0 Management API ClientSecret not set");
            }
            
            if (string.IsNullOrEmpty(secrets.Domain))
            {
                throw new ArgumentException("Auth0 Domain not set");
            }
            
            if (string.IsNullOrEmpty(secrets.ClientId))
            {
                throw new ArgumentException("Auth0 ClientId not set");
            }

            return secrets;
        }
    }
}