using Prototype.Domain.Aggregates.Site.Commands;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site
{
    public class SiteApplicationService : IMessageHandler
    {
        private readonly IRepository<SiteAggregate> _sites;

        public SiteApplicationService(IRepository<SiteAggregate> sites)
        {
            _sites = sites;
        }

        public void Handle(CreateSite c)
        {
            _sites.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(UpdateSite c)
        {
            _sites.Perform(c.Id, a => a.Update(c));
        }

        public void Handle(DeleteSite c)
        {
            _sites.Perform(c.Id, a=> a.Delete(c.Reason));
        }
    }
}