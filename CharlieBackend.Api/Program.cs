using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CharlieBackend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

            if (File.Exists(envFilePath))
            {
                DotNetEnv.Env.Load(envFilePath);
            }

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


            return builder;
        }

    }
}
