using Prototype.Databases;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers.HistoryHandlers
{
    [Priority(PriorityStages.ViewHandling_After)]
    public class SubjectHistoryViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SubjectView> _subjects;
        private readonly IDocumentCollection<SubjectView> _history;

        public SubjectHistoryViewHandler(ViewDatabase database)
        {
            _subjects = database.Subjects;
            _history = database.SubjectsHistory;
        }

        public void Handle(SubjectCreated e)
        {
            CreateRevision(e.Id);
        }

        public void Handle(SubjectUpdated e)
        {
            CreateRevision(e.Id);
        }

        public void Handle(SubjectDeleted e)
        {
            CreateRevision(e.Id);
        }

        private void CreateRevision(string subjectId)
        {
            var subject = _subjects.GetById(subjectId);
            var historyId = string.Format("{0}/{1}", subject.SubjectId, subject.Version);

            _history.UpdateOrSave(historyId, history =>
            {
                history.SubjectId = subjectId;
                history.Version = subject.Version;
                history.Name = subject.Name;
                history.Level = subject.Level;
                history.SiteId = subject.SiteId;
                history.DateOfBirth = subject.DateOfBirth;
                history.Initials = subject.Initials;
            });
        }
    }
}