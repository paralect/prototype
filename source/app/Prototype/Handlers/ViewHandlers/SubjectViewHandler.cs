using System;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class SubjectViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<SubjectView> _subjects;
        private readonly IDocumentCollection<SiteView> _sites;

        public SubjectViewHandler(ViewDatabase database)
        {
            _sites = database.Sites;
            _subjects = database.Subjects;
        }

        public void Handle(SubjectCreated e)
        {
            _subjects.Save(s =>
            {
                s.SubjectId = e.Id;
                s.DateOfBirth = e.DateOfBirth;
                s.Level = e.Level;
                s.UpdateName(e.Name, e.Initials);
                s.SiteId = e.SiteId;
                s.SiteName = GetSiteName(e.SiteId);
            });
        }

        public void Handle(SubjectUpdated e)
        {
            _subjects.Update(e.Id, s =>
            {
                s.SubjectId = e.Id;
                s.DateOfBirth = e.DateOfBirth;
                s.Level = e.Level;
                s.UpdateName(e.Name, e.Initials);

                if (string.Equals(s.SiteId, e.SiteId) == false)
                {
                    s.SiteId = e.SiteId;
                    s.SiteName = GetSiteName(e.SiteId);
                }
            });
        }

        public void Handle(SubjectDeleted e)
        {
            _subjects.Delete(e.Id);
        }

        private string GetSiteName(string siteId)
        {
            var siteName = string.Empty;
            if (string.IsNullOrEmpty(siteId) == false)
            {
                var site = _sites.GetById(siteId);

                if (site != null)
                {
                    siteName = site.Name;
                }
            }

            return siteName;
        }
    }
}