using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Prototype.Common.Interceptors;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Unity;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace Prototype.Admin
{
    public class PrototypeAdminApplication : System.Web.HttpApplication
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
                .AddHandlers(typeof(SubjectCreated).Assembly)
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }
    }
}