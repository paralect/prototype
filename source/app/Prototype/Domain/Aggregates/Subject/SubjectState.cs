using System;
using Prototype.Domain.Aggregates.Subject.Events;

namespace Prototype.Domain.Aggregates.Subject
{
    public class SubjectState
    {
        public string Id { get; set; }

        public DateTime DateOfBirth { get; set; }
        public Int32 Level { get; set; }

        public void On(SubjectCreated e)
        {
            Id = e.Id;
            Level = e.Level;
            DateOfBirth = e.DateOfBirth;
        }

        public void On(SubjectUpdated e)
        {
            Level = e.Level;
            DateOfBirth = e.DateOfBirth;
        }
    }
}