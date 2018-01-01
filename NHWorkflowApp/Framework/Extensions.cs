using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NHWorkflow.Core;
using NHWorkflow.Core.Infra;
using NHWorkflow.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHWorkflowApp.Framework
{
    public static class ServiceCollectionExtensions
    {
        public static TConfig ConfigureStartup<TConfig>(this IServiceCollection services, IConfiguration configuration)
            where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //instance of TConfig 
            var config = new TConfig();

            //bind it to the appropriate section of config 
            // TODO: which file .config or json??
            configuration.Bind(config);

            services.AddSingleton(config);

            return config;
        }

        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IMvcBuilder AddNHMvc(this IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();

            //framework supplied - sessiondata or cookie
            mvcBuilder.AddSessionStateTempDataProvider();

            // Default json serializer
            //mvcBuilder.AddJsonOptions(options =>
            //    {
            //        // MVC now serializes to CamelCase, to avoid it 
            //        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    });

            // DataBinder customization


            return mvcBuilder;

        }
        public static void AddAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".NH.Antiforgery";
            });
        }
        public static IServiceProvider ConfigurationApplicationServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // TODO: Debug for better!
            services.ConfigureStartup<NHConfig>(configuration.GetSection("NHWorkflow"));
            services.AddHttpContextAccessor();

            var engine = EngineContext.Create();
            engine.Initialize(services);

        }

        public static void AddNHAuthentication(this IServiceCollection services)
        {
            var authenticationBuilder = services.AddAuthentication(
                options =>
                {
                    options.DefaultChallengeScheme = NHCookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = NHCookieAuthenticationDefaults.ExternalAuthenticationScheme;

                }
                );

            authenticationBuilder.AddCookie(NHCookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.Name = NHCookieAuthenticationDefaults.CookiePrefix + NHCookieAuthenticationDefaults.AuthenticationScheme;
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = NHCookieAuthenticationDefaults.LoginPath;

                    options.LogoutPath = NHCookieAuthenticationDefaults.LogoutPath;
                    options.AccessDeniedPath = NHCookieAuthenticationDefaults.AccessDeniedPath;
                });

            //NoExternal Authentiation?
        }
    }

    public static class ApplicationBuilderExtensions
    {
        public static void UseNHAuthentication(this IApplicationBuilder application)
        {
            application.UseMiddleware<AuthenticationMiddleware>();

        }
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            
        }
    }

    public class NHAuthenticationStartup : INHStartup
    {
        public int Order => 500; // Before MVC 
         
        public void Configure(IApplicationBuilder application)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddNHAuthentication();
        }
    }
}
