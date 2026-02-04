using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Rws.LanguageCloud.Authentication.Jws;
using Trados.GenAI.Addon.OpenAI.DAL;
using Trados.GenAI.Addon.OpenAI.Exceptions;
using Trados.GenAI.Addon.OpenAI.Helpers;
using Trados.GenAI.Addon.OpenAI.Interfaces;
using Trados.GenAI.Addon.OpenAI.Services;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trados.GenAI.Addon.OpenAI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opts =>
            {
                // only for system.text
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddSingleton<IDatabaseContext, DatabaseContext>();
            services.AddSingleton<IHealthReporter, HealthReporter>();
            services.AddSingleton<IRepository, Repository>();
            services.AddSingleton<ITranslationRepository, TranslationRepository>();
            services.AddSingleton<IAppRegistrationRepository, AppRegistrationRepository>();
            services.AddSingleton<IDescriptorService, DescriptorService>();
            services.AddSingleton<IAccountService, AccountService>();

            // this ensures that the services are started and stopped concurrently
            //services.Configure<HostOptions>(options =>
            //{
            //    options.ServicesStartConcurrently = true;
            //    options.ServicesStopConcurrently = true;
            //});

            services.AddSingleton<ISettingsService, SettingsService>();

            // TranslationController Services
            services.AddSingleton<ITranslationEngineBuilder, TranslationEngineBuilder>();

            services.AddAuthentication(JwsDefaults.AuthenticationScheme)
                .AddJws(options =>
                {
                    //that is not implemented yet:
                    //options.TokenValidationParameters.RequireSignedTokens = Configuration["ASPNETCORE_ENVIRONMENT"] != "Development";
                    options.JwksUri = Configuration["Authorization:JwksUri"];
                    options.TokenValidationParameters.ValidIssuer = Configuration["Authorization:Issuer"];
                    options.TokenValidationParameters.ValidAudience = Configuration["baseUrl"];
                });

            BsonSerializer.RegisterSerializationProvider(new JsonNodeSerializerProvider());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This is required for intercepting the request body in validating the request signatures
            app.Use(next => context =>
            {
                context.Request.EnableBuffering(100 * 1024);
                return next(context);
            });

            app.UseHttpsRedirection();

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string message;

                        if (contextFeature.Error is AppException)
                        {
                            var exception = (AppException)contextFeature.Error;

                            context.Response.StatusCode = (int)exception.StatusCode;
                            message = JsonSerializer.Serialize(new
                            {
                                message = exception.Message,
                                errorCode = exception.ErrorCode,
                                details = exception.ExceptionDetails
                            }, JsonSettings.Default());
                        }
                        else
                        {
                            message = JsonSerializer.Serialize(new
                            {
                                errorCode = ErrorCodes.InternalError,
                                message = contextFeature.Error.Message,
                            }, JsonSettings.Default());
                        }

                        await context.Response.WriteAsync(message).ConfigureAwait(false);
                    }
                });
            });

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
