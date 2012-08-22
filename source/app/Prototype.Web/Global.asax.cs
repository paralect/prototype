using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using Prototype.Common.Interceptors;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.EventBus;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Prototype.Platform.Domain.Transitions.Mongo;
using Prototype.Platform.Mongo;
using Prototype.Platform.Unity;
using Microsoft.Practices.Unity;
using Prototype.Views;
using Uniform;
using Uniform.Mongodb;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace Prototype.Web
{
    public class PrototypeApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = HttpApplicationUnityContext.Current;

            // Order of configurators is important
            ConfigureMvc(container);
            Configuration.ConfigureSettings(container);
            Configuration.ConfigureMongoDB(container);
            ConfigureTransport(container);
            Configuration.ConfigureEventStore(container);
            Configuration.ConfigureUniform(container);
        }

        private void ConfigureMvc(IUnityContainer container)
        {
            AreaRegistration.RegisterAllAreas();

            // Registering filters
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            // Registing routes
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            // Configure resolver for MVC controllers, filters etc.
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private void ConfigureTransport(IUnityContainer container)
        {
            var dispatcher = Dispatcher.Create(d => d
                .AddHandlers(typeof(PatientCreated).Assembly)
                .AddInterceptor(typeof(LoggingInterceptor))
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }
    }
}