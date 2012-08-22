using System.Collections.Generic;
using Prototype.Domain.Aggregates.Subject;
using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Domain;

namespace Prototype.Tests.AggregateTests.SubjectTests
{
    public class when_Subject_deleted : AggregateTest<SubjectAggregate>
    {
        public override IEnumerable<IEvent> Given()
        {
            yield return new SubjectCreated() { Id = _id, Level = 25, Name = "John" };
            yield return new SubjectUpdated() { Id = _id, Level = 12, Name = "NewJohn" };
        }

        public override IEnumerable<ICommand> When()
        {
            yield return new DeleteSubject() { Id = _id, Reason = "By admin"};
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new SubjectDeleted() { Id = _id, Reason = "By admin"};
        }
    }
}