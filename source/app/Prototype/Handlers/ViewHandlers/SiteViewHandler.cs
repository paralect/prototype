using Prototype.Databases;
using Prototype.Domain.Aggregates.Site.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class SiteViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SiteView> _sites;

        public SiteViewHandler(ViewDatabase database)
        {
            _sites = database.Sites;
        }

        public void Handle(SiteCreated e)
        {
            _sites.Save(s =>
            {
                s.SiteId = e.Id;
                s.Name = e.Name;
                s.Capacity = e.Capacity;
            });
        }

        public void Handle(SiteUpdated e)
        {
            _sites.Update(e.Id, s =>
            {
                s.SiteId = e.Id;
                s.Name = e.Name;
                s.Capacity = e.Capacity;
            });
        }

        public void Handle(SiteDeleted e)
        {
            _sites.Delete(e.Id);
        }
    }
}