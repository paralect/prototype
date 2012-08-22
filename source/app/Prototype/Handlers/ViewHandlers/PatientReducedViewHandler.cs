using Prototype.Databases;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Dispatching.Attributes;
using Prototype.Views;
using Uniform;

namespace Prototype.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class PatientReducedViewHandler : IMessageHandler
    {
        private readonly IDocumentCollection<PatientViewReduced> _patients;

        public PatientReducedViewHandler(ViewDatabase database)
        {
            _patients = database.PatientsReduced;
        }

        public void Handle(PatientCreated e)
        {
            _patients.Save(patient =>
            {
                patient.PatientId = e.Id;
                patient.Name = e.Name;
            });
        }

        public void Handle(PatientUpdated e)
        {
            _patients.Update(e.Id, patient =>
            {
                patient.PatientId = e.Id;
                patient.Name = e.Name;
            });
        }

        public void Handle(PatientDeleted e)
        {
            _patients.Delete(e.Id);
        }
    }
}