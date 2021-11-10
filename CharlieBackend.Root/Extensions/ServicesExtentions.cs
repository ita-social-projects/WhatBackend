using AutoMapper.Configuration;
using CharlieBackend.Business.Models.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharlieBackend.Root.Extensions
{
    public static class ServicesExtentions
    {
        /// <summary>
        /// Adding application's custom services into container for DI.
        /// </summary>
        public static void AddBotCommands(this IServiceCollection services)
        {
            services.AddScoped<StartCommand>();
            services.AddScoped<MenuCommand>();
            services.AddScoped<HelloCommand>();
            services.AddScoped<GetMarkCommand>();
            services.AddScoped<StudentGroupsCommand>();
            services.AddScoped<CoursesCommand>();
            services.AddScoped<PersonalInfoCommand>();
            services.AddScoped<ClassmatesCommand>();
            services.AddScoped<SecretariesCommand>();
            services.AddScoped<MentorsCommand>();
            services.AddScoped<NewHomeworkCommand>();
        }
    }
}
