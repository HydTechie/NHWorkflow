using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Core.Infra
{
    // Building collection of singletons for each type? 
    public class Singleton
    {
        static readonly IDictionary<Type, object> allSingletons;

        // static constructor 
        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        protected static IDictionary<Type, object> AllSingletons 
        {
            get
            {
             return allSingletons;
            }
           
        }

    }

    public class Singleton<T> : Singleton
    {
        private static  T instance;
        private static readonly object padlock = new object();

        public static T Instance
        {
            get
            {
                return instance;
            }
            set
            {
                //simple synchronization
                lock(padlock)
                {
                    instance = value;
                    //sync the dictionary
                    AllSingletons[typeof(T)] = value;
                }
              
            }
        }

    }

    public interface INHStartup
    {
        void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration);
        void Configure(IApplicationBuilder application);
        int Order
        { get; }
    }
}
