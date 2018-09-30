using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace webapi
{
    public class Program
    {
        public static string _mongoUri;
        public static string _mongoDbName;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(); //<-- Allows for Docker Env Variables

            IConfigurationRoot configuration = builder.Build();

            _mongoUri = configuration["Settings:MongoDbUri"];
            _mongoDbName = configuration["Settings:MongoDbName"];

            #region Configuration with Docker (vs Compose)
            /*
            Injecting configuration via Docker:

            Docker Run:
                $ docker run -e Settings:ConfigurationSource=DockerRun worker

            Docker Compose or .env file:
                environment:
                 - Settings:ConfigurationSource=DockerCompose
            */
            #endregion

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
