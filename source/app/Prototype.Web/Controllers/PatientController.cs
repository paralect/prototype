using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Views;
using Prototype.Web.Models;

namespace Prototype.Web.Controllers
{
    public class PatientController : Controller
    {
        [Dependency]
        public ICommandBus Bus { get; set; }

        [Dependency]
        public MongoViewDatabase ViewDatabase { get; set; }

/*        private readonly ICommandBus _bus;
        private readonly MongoViewDatabase _viewDatabase;

        public PatientController(ICommandBus bus, MongoViewDatabase viewDatabase)
        {
            _bus = bus;
            _viewDatabase = viewDatabase;
        }*/

        public ActionResult Index()
        {
            var patients = ViewDatabase.Patients.FindAll().ToList();

            return View(new PatientPageViewModel
            {
                Patients = patients
            });
        }

        public ActionResult Create()
        {
            return View(new PatientView());
        }

        [HttpPost]
        public ActionResult Create(PatientView view)
        {
            Bus.Send(new CreatePatient()
            {
                Id = Guid.NewGuid().ToString(),
                Initials = view.Initials,
                Level = view.Level,
                DateOfBirth = view.DateOfBirth,
                Name = view.Name,
                SiteId = view.SiteId
            });

            return View(view);
        }

        public ActionResult Edit()
        {
            return View(new PatientView());
        }

        [HttpPost]
        public ActionResult Edit(PatientView view)
        {
            Bus.Send(new UpdatePatient()
            {
                Id = view.PatientId,
                Initials = view.Initials,
                Level = view.Level,
                DateOfBirth = view.DateOfBirth,
                Name = view.Name,
                SiteId = view.SiteId
            });

            return View(view);
        }

        [HttpGet]
        public ActionResult Delete(String patientId)
        {
            Bus.Send(new DeletePatient(patientId, "Removed by user from Patient page"));
            return RedirectToAction("Index");
        }
    }
}