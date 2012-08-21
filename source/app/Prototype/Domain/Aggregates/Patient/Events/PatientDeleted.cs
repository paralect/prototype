using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Patient.Events
{
    public class PatientDeleted : Event
    {
        public String Id { get; set; }
        public String Reason { get; set; }
        
        public PatientDeleted() { }
        public PatientDeleted(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}