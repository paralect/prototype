using System;

namespace Prototype.Platform.Domain
{
    public interface IEventMetadata
    {
        /// <summary>
        /// Unique Id of event
        /// </summary>
        string EventId { get; set; }

        /// <summary>
        /// Command Id of command that initiate this event
        /// </summary>
        string CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiated this event
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// Datetime when event was stored in Event Store.
        /// </summary>
        DateTime StoredDate { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name
        /// </summary>
        string TypeName { get; set; }
    }
}