using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Subject.Events
{
    public class SubjectDeleted : Event
    {
        public string Reason { get; set; }
        
        public SubjectDeleted() { }
        public SubjectDeleted(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}