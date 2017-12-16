using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DataCollector.Web.Api
{
    /// <summary>
    /// The startup point for the ASP.NET API service.
    /// </summary>
    /// <CreatedOn>16.12.2017 13:12</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    public class Startup
    {
        private readonly IConfiguration m_Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <CreatedOn>16.12.2017 21:33</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public Startup(IConfiguration configuration)
        {
            m_Configuration = configuration;
        }

        /// <summary>
        /// The configuration of the service provider.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <CreatedOn>16.12.2017 13:13</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public void ConfigureServices(IServiceCollection services)
        {
            //configure authentication method
            ConfigureJwtAuthentication(services);
            //add mvc services collection
            services.AddMvc();
        }

        /// <summary>
        /// Adds the JWT authentication support to asp.net controllers.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <CreatedOn>16.12.2017 21:31</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com",
                        ValidAudience = "yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(m_Configuration["SecurityKey"]))
                    };
                });
        }

        /// <summary>
        /// It is used to configure the app behaviors.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <CreatedOn>16.12.2017 13:13</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public void Configure(IApplicationBuilder app)
        {
            //use mvc strctural pattern
            app.UseMvc();
        }
    }
}