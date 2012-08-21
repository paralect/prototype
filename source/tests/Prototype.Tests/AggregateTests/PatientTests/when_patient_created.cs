using System.Collections.Generic;
using Prototype.Domain.Aggregates.Patient;
using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Domain;

namespace Prototype.Tests.AggregateTests.PatientTests
{
    public class when_patient_created : AggregateTest<PatientAggregate>
    {
        public override IEnumerable<IEvent> Given()
        {
            yield break; // nothing 
        }

        public override IEnumerable<ICommand> When()
        {
            yield return new CreatePatient() { Id = _id, Level = 25, Name = "John" };
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PatientCreated() { Id = _id, Level = 25, Name = "John" };
        }
    }
}