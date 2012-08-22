using System;
using System.Collections.Generic;

namespace Prototype.Platform.Domain.Transitions
{
    /// <summary>
    /// Transition - is a way to group a number of modifications (events) 
    /// for **one** Stream (usually Aggregate Root) in one atomic package, 
    /// that can be either canceled or persisted by Event Store.
    /// </summary>    
    public class Transition
    {
        /// <summary>
        /// Transition ID (StreamId, Version)
        /// </summary>
        public TransitionId Id { get; private set; }

        /// <summary>
        /// DateTime when transition was saved to the Store
        /// (or more accurately - current datetime that was set to Transition _before_ storing it to the Store)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Assembly qualified Aggregate Type name
        /// </summary>
        public string AggregateTypeId { get; set; }

        /// <summary>
        /// Events in commit
        /// </summary>
        public List<TransitionEvent> Events { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Transition(TransitionId transitionId, string aggregateTypeId, DateTime timestamp, List<TransitionEvent> events)
        {
            Id = transitionId;
            AggregateTypeId = aggregateTypeId;
            Events = events;
            Timestamp = timestamp;
        }
    }
}
