using System;
using System.Collections.Generic;

namespace Prototype.Platform.Domain
{
    public abstract class Aggregate
    {
        /// <summary>
        /// Aggregate state
        /// </summary>
        private Object _state;

        /// <summary>
        /// Aggregate version. Version 0 means that object was just created.
        /// Once object will be saved it version will be >= 1.
        /// </summary>
        protected Int32 _version;

        /// <summary>
        /// List of pending events
        /// </summary>
        protected List<IEvent> _changes = new List<IEvent>();

        /// <summary>
        /// Aggregate state
        /// </summary>
        public object State
        {
            get { return _state; }
        }

        public int Version
        {
            get { return _version; }
        }

        /// <summary>
        /// List of pending events
        /// </summary>
        public List<IEvent> Changes
        {
            get { return _changes; }
        }

        /// <summary>
        /// Establish context of aggregate
        /// </summary>
        public void Setup(Object state, Int32 version = 0)
        {
            _state = state;
            _version = version;
        }

        public void Apply(IEvent evnt)
        {
            StateSpooler.Spool(_state, evnt);
            _changes.Add(evnt);
        }

        public void Apply<TEvent>(Action<TEvent> eventBuilder) where TEvent : IEvent, new()
        {
            var evnt = new TEvent();
            eventBuilder(evnt);
            Apply(evnt);
        }
    }    

    public abstract class Aggregate<TState> : Aggregate
    {
        /// <summary>
        /// Aggregate state
        /// </summary>
        public new TState State
        {
            get { return (TState) base.State; }
        }
    }
}