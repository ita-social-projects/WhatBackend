using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace EmailSendingService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();

	}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			string envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

			if (File.Exists(envFilePath))
			{
				DotNetEnv.Env.Load(envFilePath);
			}

			var builder = Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
			return builder;
		}
	}
}
