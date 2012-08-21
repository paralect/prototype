using System;
using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Domain.Aggregates.Patient.Events;

namespace Prototype.Domain.Aggregates.Patient
{
    public class PatientAggregate : Aggregate<PatientState>
    {
        public void Create(CreatePatient c)
        {
            if (c.Level != State.Level)
                throw new ArgumentOutOfRangeException("c");

            Apply(new PatientCreated()
            {
                Id = c.Id,
                DateOfBirth = c.DateOfBirth,
                Initials = c.Initials,
                Level = c.Level,
                Name = c.Name,
                SiteId = c.SiteId
            });
        }

        public void Update(UpdatePatient c)
        {
            Apply(new PatientUpdated()
            {
                Id = c.Id,
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