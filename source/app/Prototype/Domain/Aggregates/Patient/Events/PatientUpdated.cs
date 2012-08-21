using System;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Patient.Events
{
    public class PatientUpdated : Event
    {
        public String SiteId { get; set; }
        public String Name { get; set; }
        public String Initials { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32 Level { get; set; }
    }
}