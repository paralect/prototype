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
        private readonly ICommandBus _bus;
        private readonly MongoViewDatabase _viewDatabase;

        public PatientController(ICommandBus bus, MongoViewDatabase viewDatabase)
        {
            _bus = bus;
            _viewDatabase = viewDatabase;
        }

        public ActionResult Index()
        {
            var patients = _viewDatabase.Patients.FindAll().ToList();

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
            _bus.Send(new CreatePatient()
            {
                Id = Guid.NewGuid().ToString(),
                Initials = view.Initials,
                Level = view.Level,
                DateOfBirth = view.DateOfBirth,
                Name = view.Name,
                SiteId = view.SiteId
            });

            return RedirectToAction("Index");
        }

        public ActionResult Edit(String id)
        {
            var patientView = _viewDatabase.Patients.FindOneById(id);
            return View(patientView);
        }

        [HttpPost]
        public ActionResult Edit(PatientView view)
        {
            _bus.Send(new UpdatePatient()
            {
                Id = view.PatientId,
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
            _bus.Send(new DeletePatient(id, "Removed by user from Patient page"));
            return RedirectToAction("Index");
        }
    }
}