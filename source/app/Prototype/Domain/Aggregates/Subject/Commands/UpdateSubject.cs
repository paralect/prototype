using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Subject.Commands
{
    public class UpdateSubject : Command
    {
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32 Level { get; set; }
    }
}