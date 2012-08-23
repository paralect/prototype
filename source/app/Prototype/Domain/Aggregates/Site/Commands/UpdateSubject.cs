using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Site.Commands
{
    public class UpdateSite : Command
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}