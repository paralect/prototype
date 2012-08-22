using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Subject
{
    public class SubjectApplicationService : IMessageHandler
    {
        private readonly IRepository<SubjectAggregate> _subjects;

        public SubjectApplicationService(IRepository<SubjectAggregate> subjects)
        {
            _subjects = subjects;
        }

        public void Handle(CreateSubject c)
        {
            _subjects.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(UpdateSubject c)
        {
            _subjects.Perform(c.Id, a => a.Update(c));
        }

        public void Handle(DeleteSubject c)
        {
            _subjects.Perform(c.Id, a => a.Delete(c.Reason));
        }
    }
}