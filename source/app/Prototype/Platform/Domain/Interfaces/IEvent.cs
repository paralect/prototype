using System;

namespace Prototype.Platform.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        String Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}