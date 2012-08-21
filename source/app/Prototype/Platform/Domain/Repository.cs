using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Prototype.Domain.Aggregates;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain.EventBus;
using Prototype.Platform.Domain.Transitions;
using Prototype.Platform.Domain.Transitions.Interfaces;
using Prototype.Platform.Domain.Utilities;

namespace Prototype.Platform.Domain
{
    public class Repository : IRepository
    {
        private readonly ITransitionRepository _transitionStorage;
        private readonly IEventBus _eventBus;

        public Repository(ITransitionRepository transitionStorage, IEventBus eventBus)
        {
            _transitionStorage = transitionStorage;
            _eventBus = eventBus;
        }

        public void Save(String aggregateId, Aggregate aggregate)
        {
            if (aggregateId == null)
                throw new ArgumentException(String.Format(
                    "Aggregate ID is null when trying to save {0} aggregate. Please specify aggregate ID.", aggregate.GetType().FullName));

            if (String.IsNullOrEmpty(aggregateId))
                throw new ArgumentException(String.Format(
                    "Aggregate ID is empty string when trying to save {0} aggregate. Please specify aggregate ID.", aggregate.GetType().FullName));

            var transition = CreateTransition(aggregateId, aggregate.Version, aggregate.Changes);
            _transitionStorage.SaveTransition(transition);

            if (_eventBus != null)
                _eventBus.Publish(transition.Events.Select(e => (IEvent)e.Data).ToList());
        }

        public TAggregate GetById<TAggregate>(String id)
            where TAggregate : Aggregate
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentException(String.Format(
                    "Aggregate ID was not specified when trying to get by id {0} aggregate", typeof(TAggregate).FullName));

            var aggregate = AggregateCreator.CreateAggregateRoot<TAggregate>();
            var state = AggregateCreator.CreateAggregateState(typeof (TAggregate));
            aggregate.Setup(state);

            var fromVersion = 0;
            var stream = _transitionStorage.GetTransitions(id, fromVersion, int.MaxValue);
            StateSpooler.Spool(state, stream.SelectMany(t => t.Events).Select(e => (IEvent) e.Data));

            return aggregate;
        }


        /// <summary>
        /// Perform action on aggregate with specified id.
        /// Aggregate should be already created.
        /// </summary>
        public void Perform<TAggregate>(String id, Action<TAggregate> action)
            where TAggregate : Aggregate
        {
            var aggregate = GetById<TAggregate>(id);
            action(aggregate);
            Save(id, aggregate);
        }

        /// <summary>
        /// Create changeset. Used to persist changes in aggregate
        /// </summary>
        /// <returns></returns>
        public Transition CreateTransition(String id, Int32 version, List<IEvent> changes)
        {
            if (String.IsNullOrEmpty(id))
                throw new Exception(String.Format("ID was not specified for domain object. AggregateRoot [{0}] doesn't have correct ID. Maybe you forgot to set an _id field?", this.GetType().FullName));

            var currentTime = DateTime.UtcNow;
            var transitionEvents = new List<TransitionEvent>();
            foreach (var e in changes)
            {
                e.Metadata.EventId = ObjectId.GenerateNewId().ToString();
                e.Metadata.StoredDate = currentTime;
                e.Metadata.TypeName = e.GetType().Name;

                // Take some metadata properties from command
                var command = Dispatcher.CurrentMessage as ICommand;
                if (command != null && command.Metadata != null)
                {
                    e.Metadata.CommandId = command.Metadata.CommandId;
                    e.Metadata.UserId = command.Metadata.UserId;
                }

                transitionEvents.Add(new TransitionEvent(e.GetType().AssemblyQualifiedName, e));
            }

            return new Transition(new TransitionId(id, version + 1), GetType().AssemblyQualifiedName, currentTime, transitionEvents);
        }

    }

    public class Repository<TAggregate> : Repository, IRepository<TAggregate> where TAggregate : Aggregate
    {
        public Repository(ITransitionRepository transitionStorage, IEventBus eventBus): base(transitionStorage, eventBus)
        {
            
        }

        public void Save(String aggregateId, TAggregate aggregate)
        {
            base.Save(aggregateId, aggregate);
        }

        public TAggregate GetById(String id)
        {
            return GetById<TAggregate>(id);
        }

        public void Perform(String id, Action<TAggregate> action)
        {
            Perform<TAggregate>(id, action);
        }
    }
}
