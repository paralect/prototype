using System;
using Prototype.Domain.Aggregates.Site.Commands;
using Prototype.Domain.Aggregates.Site.Events;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site
{
    public class SiteAggregate: Aggregate<SiteState>
    {
        public void Create(CreateSite c)
        {
            Apply(new SiteCreated
            {
                Id = c.Id,
                Capacity = c.Capacity,
                Name = c.Name
            });
        }

        public void Update(UpdateSite c)
        {
            Apply(new SiteUpdated
            {
                Id = c.Id,
                Name = c.Name,
                Capacity = c.Capacity
            });
        }

        public void Delete(String reason)
        {
            Apply(new SiteDeleted(State.Id, reason));
        }
    }
}