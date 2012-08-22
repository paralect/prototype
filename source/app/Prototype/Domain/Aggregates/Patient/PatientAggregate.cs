using System;
using System.ComponentModel;
using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Patient
{
    public class PatientAggregate : Aggregate<PatientState>
    {
        public void Create(CreatePatient c)
        {
            Apply(new PatientCreated()
            {
                Id = State.Id,
                DateOfBirth = c.DateOfBirth,
                Initials = c.Initials,
                Level = c.Level,
                Name = c.Name,
                SiteId = c.SiteId
            });
        }

        public void Update(UpdatePatient c)
        {
            // Example of state manipulation
            if (c.Level < State.Level)
                throw new InvalidOperationException("Level should be higher than current");

            Apply(new PatientUpdated()
            {
                Id = State.Id,
                DateOfBirth = c.DateOfBirth,
                Initials = c.Initials,
                Level = c.Level,
                Name = c.Name,
                SiteId = c.SiteId
            });            
        }

        public void Delete(String reason)
        {
            Apply(new PatientDeleted(State.Id, reason));
        }
    }
}