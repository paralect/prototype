using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Subject.Commands
{
    public class DeleteSubject : Command
    {
        public string Reason { get; set; }
        
        public DeleteSubject() { }
        public DeleteSubject(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}