using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site.Events
{
    public class SiteCreated : Event
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}