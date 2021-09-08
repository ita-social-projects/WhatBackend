using Microsoft.Extensions.DependencyInjection;

namespace CharlieBackend.Core.Extensions
{
    public static class JsonConverterExtentions
    {
        public static IMvcBuilder AddJsonSerializer(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new JsonContractResolver();
            });

            return builder;
        }
    }
}
