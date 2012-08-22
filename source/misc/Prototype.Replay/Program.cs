using Microsoft.Practices.Unity;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Unity;
using UnityServiceLocator = Microsoft.Practices.Unity.UnityServiceLocator;

namespace Prototype.Replay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = AppDomainUnityContext.Current;

            // Order of configurators is important
            Configuration.ConfigureSettings(container);
            Configuration.ConfigureMongoDB(container);
            ConfigureTransport(container);
            Configuration.ConfigureEventStore(container);
            Configuration.ConfigureUniform(container);

            var replayer = container.Resolve<Replayer>();
            replayer.Start();
        }

        private static void ConfigureTransport(IUnityContainer container)
        {
            var dispatcher = Dispatcher.Create(d => d
                // Only View and Index handlers are used when replaying
                .AddHandlers(typeof(SubjectCreated).Assembly, 
                    new[] { "Prototype.Handlers.ViewHandlers", "Prototype.Handlers.IndexHandlers" })
                .SetServiceLocator(new UnityServiceLocator(container)));

            container.RegisterType<ICommandBus, CommandBus>();
            container.RegisterInstance<IDispatcher>(dispatcher);
        }
    }
}
