using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;

namespace Prototype.Domain.Aggregates.Patient
{
    public class PatientApplicationService : IMessageHandler
    {
        private readonly IRepository<PatientAggregate> _patients;

        public PatientApplicationService(IRepository<PatientAggregate> patients)
        {
            _patients = patients;
        }

        public void Handle(CreatePatient c)
        {
            _patients.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(UpdatePatient c)
        {
            _patients.Perform(c.Id, a => a.Update(c));
        }

        public void Handle(DeletePatient c)
        {
            _patients.Perform(c.Id, a => a.Delete(c.Reason));
        }
    }
}