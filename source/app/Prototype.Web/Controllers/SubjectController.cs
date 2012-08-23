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

        public ActionResult Restore(string id, Int32 version)
        {
            string revisionId = String.Format("{0}/{1}", id, version);
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