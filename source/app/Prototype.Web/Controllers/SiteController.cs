using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Site.Commands;
using Prototype.Views;
using Prototype.Web.Models;

namespace Prototype.Web.Controllers
{
    public class SiteController : Controller
    {
        private readonly ICommandBus _bus;
        private readonly MongoViewDatabase _viewDatabase;

        public SiteController(ICommandBus bus, MongoViewDatabase viewDatabase)
        {
            _bus = bus;
            _viewDatabase = viewDatabase;
        }

        public ActionResult Index()
        {
            var subjects = _viewDatabase.Sites.FindAll().ToList();

            return View(new SitePageViewModel
                {
                    Sites = subjects
                });
        }

        public ActionResult Create()
        {
            return View(new SiteView());
        }

        [HttpPost]
        public ActionResult Create(SiteView view)
        {
            _bus.Send(new CreateSite
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = view.Name,
                    Capacity = view.Capacity
                });

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var view = _viewDatabase.Sites.FindOneById(id);
            return View(view);
        }

        [HttpPost]
        public ActionResult Edit(SiteView view)
        {
            _bus.Send(new UpdateSite
                {
                    Id = view.SiteId,
                    Name = view.Name,
                    Capacity = view.Capacity
                });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _bus.Send(new DeleteSite(id, "Removed by user from Site page"));
            return RedirectToAction("Index");
        }

        public ActionResult History(string id)
        {
            var history = _viewDatabase.SitesHistory.Find(Query.EQ("SiteId", id))
                .SetSortOrder(SortBy.Descending("Version"))
                .ToList();

            return View(new SitePageViewModel
                {
                    Sites = history
                });
        }

        public ActionResult Restore(string id, int version)
        {
            var revisionId = string.Format("{0}/{1}", id, version);
            var revision = _viewDatabase.SitesHistory.FindOneById(revisionId);

            _bus.Send(new UpdateSite
                {
                    Id = revision.SiteId,
                    Name = revision.Name,
                    Capacity = revision.Capacity
                });

            return RedirectToAction("Index");
        }
    }
}