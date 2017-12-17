using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DataCollector.Web.Api
{
    /// <summary>
    /// The starting point of the .NET Core application.
    /// </summary>
    /// <CreatedOn>16.12.2017 13:10</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    public class Program
    {
        /// <summary>
        /// This is the main method of the app.
        /// </summary>
        /// <param name="args">The arguments passed in command line.</param>
        /// <CreatedOn>16.12.2017 13:11</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        static void Main(string[] args)
        {
            //build the asp.net core host service
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseConfiguration(BuildConfiguration(args))
                .Build();
             //run the host
             host.Run();
        }

        /// <summary>
        /// Builds the configuration by taking the settings from command line
        /// and from config.json file.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns></returns>
        /// <CreatedOn>16.12.2017 13:18</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        private static IConfiguration BuildConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddCommandLine(args)
                .Build();
        }
    }
}
