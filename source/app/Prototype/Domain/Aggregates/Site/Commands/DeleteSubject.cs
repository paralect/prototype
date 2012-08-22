using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site.Commands
{
    public class DeleteSite : Command
    {
        public string Reason { get; set; }
        
        public DeleteSite() { }
        public DeleteSite(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}