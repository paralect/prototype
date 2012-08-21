using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Prototype.Platform.Dispatching
{
    public static class DispatcherConfigurationExtensions
    {
        public static DispatcherConfiguration SetServiceLocator(this DispatcherConfiguration configuration, IServiceLocator container)
        {
            configuration.ServiceLocator = container;
            return configuration;
        }

        public static DispatcherConfiguration SetMaxRetries(this DispatcherConfiguration configuration, Int32 maxRetries)
        {
            configuration.NumberOfRetries = maxRetries;
            return configuration;
        }

        public static DispatcherConfiguration AddHandlers(this DispatcherConfiguration configuration, Assembly assembly, String[] namespaces)
        {
            configuration.DispatcherHandlerRegistry.Register(assembly, namespaces);
            return configuration;
        }

        public static DispatcherConfiguration AddInterceptor(this DispatcherConfiguration configuration, Type interceptor)
        {
            configuration.DispatcherHandlerRegistry.AddInterceptor(interceptor);
            return configuration;
        }

        public static DispatcherConfiguration AddHandlers(this DispatcherConfiguration configuration, Assembly assembly)
        {
            return AddHandlers(configuration, assembly, new string[] { });
        }
    }
}