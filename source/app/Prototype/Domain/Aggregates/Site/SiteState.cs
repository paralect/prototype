using System;
using Prototype.Domain.Aggregates.Site.Events;

namespace Prototype.Domain.Aggregates.Site
{
    public class SiteState
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public int AmountOfPatiends { get; set; }

        public void On(SiteCreated e)
        {
            Name = e.Name;
            Capacity = e.Capacity;
        }

        public void On(SiteUpdated e)
        {
            Name = e.Name;
            Capacity = e.Capacity;
        }
    }
}