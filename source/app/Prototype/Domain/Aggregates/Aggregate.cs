using System;
using System.Collections.Generic;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates
{
    public abstract class Aggregate<TState>
    {
        /// <summary>
        /// Aggregate state
        /// </summary>
        protected TState _state;

        /// <summary>
        /// Aggregate version. Version 0 means that object was just created.
        /// Once object will be saved it version will be >= 1.
        /// </summary>
        protected Int32 _version;

        /// <summary>
        /// List of pending events
        /// </summary>
        protected IList<IEvent> _changes;

        /// <summary>
        /// List of pending events
        /// </summary>
        public IList<IEvent> Changes
        {
            get { return _changes; }
        }

        /// <summary>
        /// Establish context of aggregate
        /// </summary>
        public void Setup(TState state, Int32 version = 0)
        {
            _state = state;
            _version = version;
        }

        public void Apply(IEvent evnt)
        {
            Spooler.Spool(_state, evnt);
            _changes.Add(evnt);
        }

        public void Apply<TEvent>(Action<TEvent> eventBuilder) where TEvent : IEvent, new()
        {
            var evnt = new TEvent();
            eventBuilder(evnt);
            Apply(evnt);
        }
    }    
}