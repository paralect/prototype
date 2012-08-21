namespace Prototype.Platform.Domain
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        EventMetadata Metadata { get; set; }
    }
}