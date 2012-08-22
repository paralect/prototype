using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site.Events
{
    public class SiteDeleted : Event
    {
        public string Reason { get; set; }
        
        public SiteDeleted() { }
        public SiteDeleted(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}