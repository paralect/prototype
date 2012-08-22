using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site.Events
{
    public class SiteUpdated : Event
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}