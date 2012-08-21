using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Patient.Commands
{
    public class DeletePatient : Command
    {
        public String Reason { get; set; }
        
        public DeletePatient() { }
        public DeletePatient(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}