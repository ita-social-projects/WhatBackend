using CharlieBackend.Business.Services;
using CharlieBackend.Business.Services.Interfaces;
using CharlieBackend.Data.Repositories.Impl;
using CharlieBackend.Data.Repositories.Impl.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Root
{
    public class CompositionRoot
    {
        // Inject your dependencies here
        public static void injectDependencies(IServiceCollection services)
        {
            services.AddScoped<ISampleService, SampleService>();

            services.AddScoped<ISampleRepository, SampleRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
