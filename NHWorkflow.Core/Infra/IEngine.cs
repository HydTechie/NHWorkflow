using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

using AutoMapper;
using Autofac;

namespace NHWorkflow.Core.Infra
{
    public interface IEngine
    {
        void Initialize(IServiceCollection services);
        IServiceProvider ConfigureServices(IServiceCollection services, IConfigurationRoot configuration);
        // http modules like 
        void ConfigureRequestPipeline(IApplicationBuilder application);
        T Resolve<T>() where T : class;
        object Resolve(Type type);
        IEnumerable<T> ResolveAll<T>();
        object ResolveUnregistered(Type type);


    }

    public class NHWorkflowEngine : IEngine
    {
        private IServiceProvider _serviceProvider { get; set; }
        public virtual IServiceProvider ServiceProvider => _serviceProvider;

        protected IServiceProvider GetServiceProvider()
        {
            var accessor = _serviceProvider.GetService<IHttpContextAccessor>();
            var context = accessor.HttpContext;
             
            return context != null ? context.RequestServices : ServiceProvider;
        }
        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            throw new NotImplementedException();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var typeFinder = new AppDomainTypeFinder();
            var startupConfigurations = typeFinder.FindClassesOfType<INHStartup>();

            var instances = startupConfigurations
                           .Select(startup => (INHStartup)Activator.CreateInstance(startup))
                           .OrderBy(startup => startup.Order);
             
            foreach(var instance in instances)
            {
                instance.ConfigureServices(services, configuration);
            }

            // register mapper configurations
            AddAutoMapper(services, typeFinder);

            //register dependencies 
            var nhConfig = services.BuildServiceProvider().GetService<NHConfig>();
            RegisterDependencies()




        }
        protected virtual void RegisterDependencies(NHConfig config, IServiceCollection services, IConfigurationRoot configuration, ITypeFinder typeFinder)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();
            containerBuilder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

        }
        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {
            var mapperConfigurations = typeFinder.FindClassesOfType<IMapperProfile>();

          var instances =  mapperConfigurations
                .Select(mapperConfiguration => (IMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            var config = new MapperConfiguration(
                cfg =>
                {
                    foreach(var instance in instances)
                    {
                        cfg.AddProfile(instance.GetType());
                    }
                }
                );


            services.AddAutoMapper();
            AutoMapperConfiguration.Init(config);


        }

        public void Initialize(IServiceCollection services)
        {
            // most of API providers need TLS 1.2 now a days
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //set base application path 
            var provider = services.BuildServiceProvider();
            var nhConfig = provider.GetRequiredService<NHConfig>();

        }

        public T Resolve<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public object ResolveUnregistered(Type type)
        {
            throw new NotImplementedException();
        }
    }

    public class EngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            if(Singleton<IEngine>.Instance == null)
            {
                Singleton<IEngine>.Instance = new NHWorkflowEngine();
            }
            return Singleton<IEngine>.Instance;
        }

        public static IEngine Current
        {
            get
            {
                return Create();

            }
            
        }
    }
}
