using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
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
using Prototype.Views;
using Uniform;
using Uniform.Mongodb;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace Prototype.Replay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = AppDomainUnityContext.Current;

            ConfigureSettings(container);
            ConfigureMongoDB(container);
            ConfigureTransport(container);
            ConfigureEventStore(container);
            ConfigureUniform(container);

            var replayer = container.Resolve<Replayer>();
            replayer.Start();
        }

        private static void ConfigureSettings(IUnityContainer container)
        {
            container.RegisterInstance(new PrototypeSettings()
            {
                MongoEventsConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_events",
                MongoViewConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_view"
            });
        }

        private static void ConfigureMongoDB(IUnityContainer container)
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

        private static void ConfigureTransport(IUnityContainer container)
        {
            var dispatcher = Dispatcher.Create(d => d
                // Only View and Index handlers are used when replaying
                .AddHandlers(typeof(PatientCreated).Assembly, 
                    new[] { "Prototype.Handlers.ViewHandlers", "Prototype.Handlers.IndexHandlers" })
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }

        private static void ConfigureEventStore(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();
            var dispatcher = container.Resolve<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.RegisterInstance<ITransitionRepository>(transitionsRepository)
                .RegisterInstance<IEventBus>(new DispatcherEventBus(dispatcher))
                .RegisterType<IRepository, Repository>()
                .RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }

        private static void ConfigureUniform(IUnityContainer container)
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
