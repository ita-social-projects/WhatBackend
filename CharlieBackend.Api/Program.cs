using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NuGet.Frameworks;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.File;
using Serilog.Settings.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CharlieBackend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json")
            //        .Build();

            //Configuration for Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(HostConfigurationBuilder(args).Build(), sectionName: "Logging")
                    .Enrich.FromLogContext()
                    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, 
                                   retainedFileCountLimit: null, fileSizeLimitBytes: null)
                    .CreateLogger();

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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envFilePath))
            {
                DotNetEnv.Env.Load(envFilePath);
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

        public static IConfigurationBuilder  HostConfigurationBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);

            return configuration;
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .UseSerilog() //Serilog added
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }


   
}
