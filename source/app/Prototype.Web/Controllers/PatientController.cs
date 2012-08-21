using System.Web.Mvc;
using Prototype.Views;
using Prototype.Web.Models;

namespace Prototype.Web.Controllers
{
    public class PatientController : Controller
    {
        public ActionResult Index()
        {
            return View(new PatientPageViewModel());
        }

        public ActionResult Create()
        {
            return View(new PatientView());
        }

        public ActionResult Edit()
        {
            return View(new PatientView());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}