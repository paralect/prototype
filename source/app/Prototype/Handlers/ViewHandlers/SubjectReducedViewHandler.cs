using Prototype.Databases;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class SubjectReducedViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SubjectViewReduced> _subjects;

        public SubjectReducedViewHandler(ViewDatabase database)
        {
            _subjects = database.SubjectsReduced;
        }

        public void Handle(SubjectCreated e)
        {
            _subjects.Save(s =>
            {
                s.SubjectId = e.Id;
                s.Name = e.Name;
            });
        }

        public void Handle(SubjectUpdated e)
        {
            _subjects.Update(e.Id, s =>
            {
                s.SubjectId = e.Id;
                s.Name = e.Name;
            });
        }

        public void Handle(SubjectDeleted e)
        {
            _subjects.Delete(e.Id);
        }
    }
}