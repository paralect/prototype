using System;
using System.ComponentModel;
using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Subject
{
    public class SubjectAggregate : Aggregate<SubjectState>
    {
        public void Create(CreateSubject c)
        {
            Apply(new SubjectCreated
            {
                Id = c.Id,
                DateOfBirth = c.DateOfBirth,
                Initials = c.Initials,
                Level = c.Level,
                Name = c.Name,
                SiteId = c.SiteId
            });
        }

        public void Update(UpdateSubject c)
        {
            // Example of state manipulation
            if (c.Level < State.Level)
                throw new InvalidOperationException("Level should be higher than current");

            Apply(new SubjectUpdated
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
            Apply(new SubjectDeleted(State.Id, reason));
        }
    }
}