using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abe.UnitTests;
using NUnit.Framework;
using Microsoft.Practices.Unity;
using Prototype.Domain.Aggregates;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Domain.EventBus;
using Prototype.Platform.Domain.Transitions;
using Prototype.Platform.Domain.Transitions.InMemory;
using Prototype.Platform.Domain.Transitions.Interfaces;

namespace Prototype.Tests
{
    [TestFixture]
    public abstract class AggregateTest<TAggregate> where TAggregate : Aggregate
    {
        protected String _id;
        protected TAggregate _aggregate;
        protected IEventBus _eventBus;
        protected List<IEvent> _actualEvents;
        protected Exception _lastExceptions;
        protected Dispatcher _eventDispatcher;
        protected bool _createNewDatabase;

        protected IUnityContainer _container;

        public abstract IEnumerable<IEvent> Given();
        public abstract IEnumerable<ICommand> When();
        public abstract IEnumerable<IEvent> Expected();
        public virtual void Setup(IUnityContainer container) {}

        [TestFixtureSetUp]
        public void SetupAggregateTest()
        {
            _container = new UnityContainer();
            SetupContainerInternal(_container);
            Setup(_container);
            _eventBus = _container.Resolve<IEventBus>();
        }

        private void SetupContainerInternal(IUnityContainer container)
        {
            var transitionsRepository = new InMemoryTransitionRepository();
            container.RegisterInstance<ITransitionRepository>(transitionsRepository);
            container.RegisterInstance<IEventBus>(new InMemoryEventBus());
            container.RegisterType<IRepository, Repository>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            var dispatcher = Dispatcher.Create(d => d
                .AddHandlers(typeof (PatientCreated).Assembly)
                .SetServiceLocator(new UnityServiceLocator(container))
            );

            container.RegisterInstance<IDispatcher>(dispatcher);
        }

        [SetUp]
        public void Prepare()
        {
            _id = Guid.NewGuid().ToString();
            _actualEvents = new List<IEvent>();
            PrepareEvents();
            var bus = _container.Resolve<IDispatcher>();
            foreach (var command in When())
            {
                bus.Dispatch(command);
            }

            _actualEvents = ((InMemoryEventBus)_eventBus).Events;
        }

        [Test]
        public void Test()
        {
            Validate();
        }

        protected virtual void PrepareEvents()
        {
            var store = GetInstance<ITransitionRepository>();
            var given = Given();
            var aggregates = new Dictionary<String, List<IEvent>>();

            foreach (var evnt in given)
            {
                var aggregateId = evnt.Id;
                var id = aggregateId ?? _id;

                List<IEvent> list;
                if (!aggregates.TryGetValue(id, out list))
                    aggregates[id] = list = new List<IEvent>();

                list.Add(evnt);
            }

            foreach (var aggregate in aggregates)
            {
                var transitionEvents = aggregate.Value.Select(e => new TransitionEvent("", e)).ToList();
                var transition = new Transition(new TransitionId(aggregate.Key, 1), typeof (TAggregate).FullName, DateTime.Now, transitionEvents);
                store.AppendTransition(transition);
            }
        }

        public void Validate(params string[] exclude)
        {
            var expectedEvents = Expected().ToList();

            if (expectedEvents.Count == 1 && expectedEvents[0].GetType() == typeof(ExceptionEvent))
            {
                var exception = (ExceptionEvent) expectedEvents[0];

                if (_lastExceptions == null)
                    throw new Exception(String.Format("Exception of type '{0}' expected.", exception.ExceptionType));

                // take inner exception, if this is TargetInvocationException
                _lastExceptions = _lastExceptions is TargetInvocationException ? _lastExceptions.InnerException : _lastExceptions;
                
                if (exception.ExceptionType != _lastExceptions.GetType())
                    throw new Exception(String.Format("Exception of type '{0}' expected, but '{1}' was rised", exception.ExceptionType.FullName, _lastExceptions.GetType().FullName));

                return;
            }

            if (_lastExceptions != null)
                throw _lastExceptions;

            Assert.AreEqual(expectedEvents.Count, _actualEvents.Count, "Incorrect number of expected events");

            for (int i = 0; i < _actualEvents.Count; i++)
            {
                var actual = _actualEvents[i];
                var expected = expectedEvents[i];

                if (expected.GetType() == typeof(ExceptionEvent))
                    throw new Exception("ExceptionEvent can be only one expected event.");

                var excludeList = new List<string>(exclude);
                excludeList.Add("Metadata");
                var equal = ObjectComparer.AreObjectsEqual(expected, actual, IgnoreList.Create(excludeList.ToArray())); // ignore property with Metadata name
                Assert.IsTrue(equal);
            }
        }

        public void AssertException<TException>()
        {
            if (_lastExceptions == null)
                throw new Exception(String.Format("Exception [{0}] expected.", typeof(TException).FullName));

            if (_actualEvents.Count > 0)
                throw new Exception(String.Format("Events shouldn't be published because of exception"));
        }

        public ExceptionEvent Exception<TException>()
        {
            return new ExceptionEvent(typeof (TException));
        }

        /// <summary>
        /// Get instanse of specified type
        /// </summary>
        public T GetInstance<T>()
        {
            return _container.Resolve<T>();
        }

        protected T Create<T>(Action<T> optionalBuilder, Action<T> builder)
        {
            var message = Activator.CreateInstance<T>();

            builder(message);

            if (optionalBuilder != null)
                optionalBuilder(message);

            return message;
        }
    }

    public class ExceptionEvent : Event
    {
        public Type ExceptionType { get; set; }

        public ExceptionEvent(Type exception)
        {
            ExceptionType = exception;
        }
    }
}