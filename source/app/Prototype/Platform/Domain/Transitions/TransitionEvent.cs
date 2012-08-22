using System;
using System.Collections.Generic;

namespace Prototype.Platform.Domain.Transitions
{
    /// <summary>
    /// 
    /// </summary>
    public class TransitionEvent
    {
        /// <summary>
        /// Type of event. By default this is a fully qualified name of CLR type.
        /// But can be anything that can help identify event type during deserialization phase.
        /// </summary>
        public string TypeId { get; private set; }

        /// <summary>
        /// Data or body of event
        /// </summary>
        public Object Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TransitionEvent(String typeId, Object data)
        {
            TypeId = typeId;
            Data = data;
        }
    }
}
