using System.Collections.Generic;
using Prototype.Domain.Aggregates.Subject;
using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Domain;

namespace Prototype.Tests.AggregateTests.SubjectTests
{
    public class when_subject_created : AggregateTest<SubjectAggregate>
    {
        public override IEnumerable<IEvent> Given()
        {
            yield break; // nothing 
        }

        public override IEnumerable<ICommand> When()
        {
            yield return new CreateSubject() { Id = _id, Level = 25, Name = "John" };
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new SubjectCreated() { Id = _id, Level = 25, Name = "John" };
        }
    }
}