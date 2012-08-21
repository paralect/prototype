using System.Collections.Generic;
using Prototype.Domain.Aggregates.Patient;
using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Domain;

namespace Prototype.Tests.AggregateTests.PatientTests
{
    public class when_patient_deleted : AggregateTest<PatientAggregate>
    {
        public override IEnumerable<IEvent> Given()
        {
            yield return new PatientCreated() { Id = _id, Level = 25, Name = "John" };
            yield return new PatientUpdated() { Id = _id, Level = 12, Name = "NewJohn" };
        }

        public override IEnumerable<ICommand> When()
        {
            yield return new DeletePatient() { Id = _id, Reason = "By admin"};
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new PatientDeleted() { Id = _id, Reason = "By admin"};
        }
    }
}