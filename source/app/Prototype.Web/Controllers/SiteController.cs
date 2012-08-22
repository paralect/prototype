using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;
using Prototype.Databases;
using Prototype.Domain.Aggregates.Site.Commands;
using Prototype.Views;
using Prototype.Web.Models;

namespace Prototype.Web.Controllers
{
    public class SiteController: Controller
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
    }
}