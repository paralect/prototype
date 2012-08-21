using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.EventBus;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Prototype.Platform.Domain.Transitions.Mongo;
using Prototype.Platform.Mongo;
using Prototype.Platform.Unity;
using Microsoft.Practices.Unity;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace Prototype.Web
{
    public class PrototypeApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            var container = HttpApplicationUnityContext.Current;
            ConfigureSettings(container);
            ConfigureMvc(container);
            ConfigureMongoDB(container);
            ConfigurePlatform(container);
        }

        private void ConfigureSettings(IUnityContainer container)
        {
            var settings = new PrototypeSettings()
            {
                MongoEventsConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_events",
                MongoViewConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_view"
            };

            container.RegisterInstance(settings);
        }

        private void ConfigureMvc(IUnityContainer container)
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Configure resolver for MVC controllers, filters etc.
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private void ConfigureMongoDB(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();
            container.RegisterInstance(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
            container.RegisterInstance(new MongoEventsDatabase(settings.MongoEventsConnectionString));

            // Register bson serializer conventions
            var myConventions = new ConventionProfile();
            myConventions.SetIdMemberConvention(new NoDefaultPropertyIdConvention());
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            BsonClassMap.RegisterConventions(myConventions, t => true);
            DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.UtcInstance;
        }

        private void ConfigurePlatform(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);
            
            var dispatcher = Dispatcher.Create(d => d
                .AddHandlers(typeof (PatientCreated).Assembly)
                .SetServiceLocator(new UnityServiceLocator(container))
            );

            container.RegisterInstance<ITransitionRepository>(transitionsRepository);
            container.RegisterInstance<IEventBus>(new DispatcherEventBus(dispatcher));
            container.RegisterType<IRepository, Repository>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}