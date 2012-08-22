using Prototype.Domain.Aggregates.Site.Commands;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site
{
    public class SiteApplicationService : IMessageHandler
    {
        private readonly IRepository<SiteAggregate> _subjects;

        public SiteApplicationService(IRepository<SiteAggregate> subjects)
        {
            _subjects = subjects;
        }

        public void Handle(CreateSite c)
        {
            _subjects.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(UpdateSite c)
        {
            _subjects.Perform(c.Id, a => a.Update(c));
        }

        public void Handle(DeleteSite c)
        {
            _subjects.Perform(c.Id, a=> a.Delete(c));
        }
    }
}