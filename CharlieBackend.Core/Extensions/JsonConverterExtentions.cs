using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CharlieBackend.Core.Extensions
{
    public static class JsonConverterExtentions
    {
        public static IMvcBuilder AddJsonConverter(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new JsonContractResolver();
            });

            return builder;
        }
    }
}
