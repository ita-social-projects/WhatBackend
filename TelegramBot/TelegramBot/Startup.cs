using CharlieBackend.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Telegram.Bot;
using TelegramBot.Services;
using TelegramBot.Services.Interfaces;
using TelegramBot.Utils;

namespace TelegramBot
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
            var botConfig = Configuration.GetSection("BotConfiguration").Get<BotSettings>();

            services.AddHostedService<ConfigureWebhook>();

            services.Configure<ApplicationSettings>(Configuration);

            services.AddHttpContextAccessor();

            services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));

            services.AddSingleton<IUserDataService, UserDataService>();
            services.AddScoped<HandleUpdateService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddHttpClient<IHttpUtil, HttpUtil>("HttpUtil", client =>
            {
#if DEBUG  
                client.BaseAddress = new Uri(Configuration.GetSection("Urls:Api:Http").Value);
#else
                client.BaseAddress = new Uri(configuration.GetSection("Urls:Api:Https").Value);
#endif
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

            services.AddScoped<IApiUtil, ApiUtil>();

            services.AddAuthorization();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Message/Update");
            });

            services.AddControllers().AddJsonSerializer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
