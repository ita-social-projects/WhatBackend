using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace CharlieBackend.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Main(string[] args)
        {
            //Configuration for Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(HostConfigurationBuilder(args).Build(), sectionName: "Logging")
                    .Enrich.FromLogContext()
                    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, 
                                   retainedFileCountLimit: 2, fileSizeLimitBytes: null)
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .CreateLogger();

            Console.Title = "CharlieBackend.Api";

            try
            {
                Log.Information("Application has started...");

                var host = CreateHostBuilder(args).Build();
                       
                host.Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envFilePath))
            {
                DotNetEnv.Env.Load(envFilePath);
            }
            else
            {
                Log.Warning(".Env file must be configured");
            }

            var builder = Host.CreateDefaultBuilder(args)
                    .ConfigureHostConfiguration(x => HostConfigurationBuilder(args))
                    .UseSerilog()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });

            return builder;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IConfigurationBuilder  HostConfigurationBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);

            return configuration;
        }
    }
}