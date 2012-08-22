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

            ConfigureSettings(container);
            ConfigureMvc(container);
            ConfigureMongoDB(container);
            ConfigureTransport(container);
            ConfigureEventStore(container);
            ConfigureUniform(container);
        }

        private void ConfigureSettings(IUnityContainer container)
        {
            container.RegisterInstance(new PrototypeSettings()
            {
                MongoEventsConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_events",
                MongoViewConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_view"
            });
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

        private void ConfigureTransport(IUnityContainer container)
        {
            var dispatcher = Dispatcher.Create(d => d
                .AddHandlers(typeof(PatientCreated).Assembly)
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }

        private void ConfigureEventStore(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();
            var dispatcher = container.Resolve<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.RegisterInstance<ITransitionRepository>(transitionsRepository)
                .RegisterInstance<IEventBus>(new DispatcherEventBus(dispatcher))
                .RegisterType<IRepository, Repository>()
                .RegisterType(typeof (IRepository<>), typeof (Repository<>));
        }

        private void ConfigureUniform(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();

            // 1. Create databases
            var mongodbDatabase = new MongodbDatabase(settings.MongoViewConnectionString);

            // 2. Configure uniform 
            var uniform = UniformDatabase.Create(config => config
                .RegisterDocuments(typeof(PatientView).Assembly)
                .RegisterDatabase(ViewDatabases.Mongodb, mongodbDatabase));

            container.RegisterInstance(uniform);            
        }
    }
}