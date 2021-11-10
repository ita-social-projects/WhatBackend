using AutoMapper;
using CharlieBackend.Api.Extensions;
using CharlieBackend.Api.Middlewares;
using CharlieBackend.Api.VersioningHelpers;
using CharlieBackend.Business.Options;
using CharlieBackend.Business.Services.Notification;
using CharlieBackend.Core.DTO.Result;
using CharlieBackend.Core.Extensions;
using CharlieBackend.Core.Mapping;
using CharlieBackend.Core.Models.ResultModel;
using CharlieBackend.Data;
using CharlieBackend.Root;
using EasyNetQ;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MySql;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(
                new MySqlStorage(
                    Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(10),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = true,
                        DashboardJobListLimit = 25000,
                        TransactionTimeout = TimeSpan.FromMinutes(1),
                        TablesPrefix = "Hangfire",
                    }
                )
            )
            );

            services.AddHangfireServer();

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

            services.AddHttpContextAccessor();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = c =>
                    {
                        var errors = string.Join('\n', c.ModelState.Values
                            .Where(v => v.Errors.Count > 0)
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage));

                        return new BadRequestObjectResult(new ErrorDto
                        {
                            Error = new ErrorData
                            {
                                Code = ErrorCode.ValidationError,
                                Message = errors
                            }
                        });
                    };
                })
                .AddJsonSerializer()
                .AddFluentValidation(options =>
                {
                    options.ValidatorOptions.CascadeMode = CascadeMode.Stop;
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
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

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(c =>
            {
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

            services.ConfigureOptions<ConfigureSwaggerOptions>();

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddFluentValidationRulesToSwagger(options =>
            {
                options.SetNotNullableIfMinLengthGreaterThenZero = true;
                options.UseAllOffForMultipleRules = true;
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            ApplicationContext dbContext,
            IApiVersionDescriptionProvider provider,
            IServiceProvider serviceProvider)
        {
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(serviceProvider));

            dbContext.Database.EnsureCreated();

            CompositionRoot.Configure(serviceProvider, Configuration);

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

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

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"WHAT Project API {description.GroupName}");
                }
            });

            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHangfireDashboard("/hangfire");
            }

            //Added Serilog to the app�s middleware pipeline
            app.UseSerilogRequestLogging();

            app.UseMiddleware<ExceptionHandleMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<IsAccountActiveMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
