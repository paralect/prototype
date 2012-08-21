using System;
using System.Collections.Generic;
using Prototype.Platform.Domain.Transitions;

namespace Prototype.Platform.Domain
{
    public abstract class AggregateRoot
    {
        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        protected string _id;

        /// <summary>
        /// Aggregate version. Version 0 means that object was just created.
        /// Once object will be saved it version will be >= 1.
        /// </summary>
        private int _version = 0;

        /// <summary>
        /// List of changes (i.e. list os pending events)
        /// </summary>
        private readonly List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Unique identifier of Aggregate Root
        /// </summary>
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Aggregate version
        /// </summary>
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public List<IEvent> Changes
        {
            get { return _changes; }
        }

        protected AggregateRoot() { }

        /// <summary>
        /// Load aggreagate from list of transitions
        /// </summary>
        public void LoadFromTransitions(IEnumerable<Transition> transitions)
        {
            foreach (var transition in transitions)
            {
                foreach (var evnt in transition.Events)
                {
                    Apply((IEvent)evnt.Data, false);
                }

                _version = transition.Id.Version;
            }
        }

        /// <summary>
        /// Load aggregate from events
        /// </summary>
        public void LoadFromEvents(IEnumerable<IEvent> events, Int32 version = 1)
        {
            foreach (var evnt in events)
            {
                Apply(evnt, false);
            }

            _version = version;            
        }

        /// <summary>
        /// Apply event on aggregate 
        /// </summary>
        public void Apply(IEvent evnt)
        {
            Apply(evnt, true);
        }

        public void Apply<TEvent>(Action<TEvent> eventBuilder) where TEvent : IEvent, new()
        {
            var evnt = new TEvent();
            eventBuilder(evnt);
            Apply(evnt);
        }

        private void Apply(IEvent evnt, bool isNew)
        {
            StateSpooler.Spool(this, evnt);
            
            if (isNew) 
                _changes.Add(evnt);
        }
    }
}
