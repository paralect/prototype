using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Views;
using Prototype.Web.Models;

namespace Prototype.Web.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ICommandBus _bus;
        private readonly MongoViewDatabase _viewDatabase;

        public SubjectController(ICommandBus bus, MongoViewDatabase viewDatabase)
        {
            _bus = bus;
            _viewDatabase = viewDatabase;
        }

        public ActionResult Index()
        {
            var subjects = _viewDatabase.Subjects.FindAll().ToList();

            return View(new SubjectPageViewModel
            {
                Subjects = subjects
            });
        }

        public ActionResult Create()
        {
            return View(CreateModel());
        }

        [HttpPost]
        public ActionResult Create([Bind(Prefix  = "SubjectView")] SubjectView view)
        {
            ValidateSubject(view);

            if (ModelState.IsValid == false)
            {
                return View(CreateModel(view));
            }

            _bus.Send(new CreateSubject
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Initials = view.Initials,
                Level = view.Level,
                DateOfBirth = view.DateOfBirth,
                Name = view.Name,
                SiteId = view.SiteId
            });

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var subjectView = _viewDatabase.Subjects.FindOneById(id);
            return View(CreateModel(subjectView));
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "SubjectView")] SubjectView view)
        {
            ValidateSubject(view);

            if (ModelState.IsValid == false)
            {
                return View(CreateModel(view));
            }

            _bus.Send(new UpdateSubject
            {
                Id = view.SubjectId,
                Initials = view.Initials,
                Level = view.Level,
                DateOfBirth = view.DateOfBirth,
                Name = view.Name,
                SiteId = view.SiteId
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(String id)
        {
            _bus.Send(new DeleteSubject(id, "Removed by user from Subject page"));
            return RedirectToAction("Index");
        }

        public ActionResult History(String id)
        {
            var history = _viewDatabase.SubjectsHistory.Find(Query.EQ("SubjectId", id))
                .SetSortOrder(SortBy.Descending("Version"))
                .ToList();

            return View(new SubjectPageViewModel
            {
                Subjects = history
            });            
        }

        public ActionResult Restore(string id, int version)
        {
            var revisionId = string.Format("{0}/{1}", id, version);
            var revision = _viewDatabase.SubjectsHistory.FindOneById(revisionId);

            _bus.Send(new UpdateSubject
            {
                Id = revision.SubjectId,
                Initials = revision.Initials,
                Level = revision.Level,
                DateOfBirth = revision.DateOfBirth,
                Name = revision.Name,
                SiteId = revision.SiteId
            });

            return RedirectToAction("Index");
        }

        private void ValidateSubject(SubjectView view)
        {
            if (string.IsNullOrEmpty(view.SiteId)) 
                return;

            var site = _viewDatabase.Sites.FindOneById(view.SiteId);

            if (site == null) 
                return;

            var amountOfSubjects = _viewDatabase.Subjects.Count(Query.EQ("SiteId", view.SiteId));

            if (site.Capacity <= amountOfSubjects)
            {
                ModelState.AddModelError("SiteId",
                                         "Capacity limit is reached for selected site. Please select different site.");
            }
        }

        private SubjectViewModel CreateModel(SubjectView view = null)
        {
            view = view ?? new SubjectView();

            var model = new SubjectViewModel
            {
                SubjectView = view,
                Sites = _viewDatabase.Sites.FindAll().ToList()
            };

            return model;
        }
    }
}