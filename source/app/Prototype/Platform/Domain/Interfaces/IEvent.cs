using System;

namespace Prototype.Platform.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        string Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}