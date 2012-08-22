using Microsoft.Practices.Unity;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using Prototype.Databases;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.EventBus;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Prototype.Platform.Domain.Transitions.Mongo;
using Prototype.Platform.Mongo;
using Prototype.Views;
using Uniform;
using Uniform.Mongodb;

namespace Prototype
{
    public static class Configuration
    {
        public static void ConfigureSettings(IUnityContainer container)
        {
            container.RegisterInstance(new PrototypeSettings()
            {
                MongoEventsConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_events",
                MongoViewConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_view",
                MongoLogsConnectionString = "mongodb://admin(admin):adminpwd0375@localhost:27017/prototype_logs"
            });
        }

        public static void ConfigureMongoDB(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();
            container.RegisterInstance(new MongoViewDatabase(settings.MongoViewConnectionString).EnsureIndexes());
            container.RegisterInstance(new MongoLogsDatabase(settings.MongoLogsConnectionString).EnsureIndexes());
            container.RegisterInstance(new MongoEventsDatabase(settings.MongoEventsConnectionString));

            // Register bson serializer conventions
            var myConventions = new ConventionProfile();
            myConventions.SetIdMemberConvention(new NoDefaultPropertyIdConvention());
            myConventions.SetIgnoreExtraElementsConvention(new AlwaysIgnoreExtraElementsConvention());
            BsonClassMap.RegisterConventions(myConventions, type => true);
            DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.UtcInstance;
        }

        public static void ConfigureEventStore(IUnityContainer container)
        {
            var settings = container.Resolve<PrototypeSettings>();
            var dispatcher = container.Resolve<IDispatcher>();

            var transitionsRepository = new MongoTransitionRepository(settings.MongoEventsConnectionString);

            container.RegisterInstance<ITransitionRepository>(transitionsRepository)
                .RegisterInstance<IEventBus>(new DispatcherEventBus(dispatcher))
                .RegisterType<IRepository, Repository>()
                .RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void ConfigureUniform(IUnityContainer container)
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