using Prototype.Databases;
using Prototype.Domain.Aggregates.Site.Events;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers.HistoryHandlers
{
    [Priority(PriorityStages.ViewHandling_After)]
    public class SiteHistoryViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SiteView> _sites;
        private readonly IDocumentCollection<SiteView> _history;

        public SiteHistoryViewHandler(ViewDatabase database)
        {
            _sites = database.Sites;
            _history = database.SitesHistory;
        }

        public void Handle(SiteCreated e)
        {
            CreateRevision(e.Id);
        }

        public void Handle(SiteUpdated e)
        {
            CreateRevision(e.Id);
        }

        public void Handle(SiteDeleted e)
        {
            CreateRevision(e.Id);
        }

        private void CreateRevision(string siteId)
        {
            var site = _sites.GetById(siteId);
            var historyId = string.Format("{0}/{1}", site.SiteId, site.Version);

            _history.UpdateOrSave(historyId, history =>
            {
                history.SiteId = siteId;
                history.Version = site.Version;
                history.Name = site.Name;
                history.Capacity = site.Capacity;
            });
        }
    }
}