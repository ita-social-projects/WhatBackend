using System;
using Serilog;
using EasyNetQ;
using System.IO;
using AutoMapper;
using System.Reflection;
using CharlieBackend.Root;
using CharlieBackend.Core;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using CharlieBackend.Core.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using CharlieBackend.Api.Extensions;
using Microsoft.IdentityModel.Tokens;
using CharlieBackend.Api.Middlewares;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using CharlieBackend.Business.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CharlieBackend.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            CompositionRoot.InjectDependencies(services, Configuration);

            var authOptions = new AuthOptions();
            Configuration.GetSection("AuthOptions").Bind(authOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = authOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddCors();

            services.AddControllers()
                .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                    });

            // EasyNetQ Congiguration through extension
            services.AddEasyNetQ(Configuration.GetConnectionString("RabbitMQ"));

            //AzureStorageBlobs Congiguration through extension
            services.AddAzureStorageBlobs(Configuration.GetConnectionString("AzureBlobsAccessKey"));

            // AutoMapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelMappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WHAT Project API", Version = "07.12.2020" });
                c.ExampleFilters();
                c.OperationFilter<AddResponseHeadersFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

               c.IncludeXmlComments(xmlPath);

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.IncludeXmlComments(xmlPath); 

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Specify a Bearer token. \nExample: Bearer yJhbGciOiJIUzI1iIsInR5cCI6IkpXVCJ9",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.Configure<SwaggerOptions>(c => c.SerializeAsV2 = true);
            
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseMiddleware<ExceptionHandleMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WHAT Project API");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Added Serilog to the app�s middleware pipeline
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<IsAccountActiveMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
