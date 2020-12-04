using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Api.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureInfrastructureSecrets(services);

            services.AddHealthChecks()
                .AddCheck<CircuitBreakerHealthCheck>("circuit_breaker");

            // AddControllers only register controllers in the same assembly as it is running from.
            // when running from the integration test it will try to registers controllers from that assembly
            // AddApplicationPart fixes it, it can reference any class in the Api project
            services.AddControllers()
                .AddApplicationPart(typeof(UsersController).Assembly);

            ConfigureAuthentication(services);
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(),
                    Title = "Publate API",
                    Description = "",
                    Contact = new OpenApiContact
                    {
                        Name = "Frederik Banke",
                        Email = "hi@frederikbanke.com",
                        Url = new Uri("https://www.frederikbanke.com")
                    }
                });

                const string securitySchemeName = "Bearer";
                c.AddSecurityDefinition(securitySchemeName, 
                    new OpenApiSecurityScheme{
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, 
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{ 
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = securitySchemeName,
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });
            
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityService, IdentityService>();

        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            var auth0 = Configuration
                .GetSection("Auth0")
                .Get<Infrastructure.Auth0.Settings>();

            if (auth0 == null)
            {
                return;
            }
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{auth0.Domain}/";
                options.Audience = auth0.ClientId;
            });
        }

        private void ConfigureInfrastructureSecrets(IServiceCollection services)
        {
            services.Configure<Infrastructure.Auth0.Settings>(Configuration.GetSection("Auth0"));
            services.Configure<Infrastructure.LinkedIn.Settings>(Configuration.GetSection("LinkedIn"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Publate V1");
                c.RoutePrefix = string.Empty;
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
