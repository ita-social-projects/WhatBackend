using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CharlieBackend.AdminPanel
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // check environment is set
            if (String.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
            {
                Console.WriteLine("ASPNETCORE_ENVIRONMENT env variable must be set!");
                return ;
            }

            Console.Title = "CharlieBackend.AdminPanel";

            var host = CreateHostBuilder(args).Build();

            host.Run();

            Console.Title = "CharlieBackend.AdminPanel";

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                    .ConfigureHostConfiguration(x => HostConfigurationBuilder(args))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });

            return builder;
        }

        public static IConfigurationBuilder HostConfigurationBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);

            return configuration;
        }
    }
}
