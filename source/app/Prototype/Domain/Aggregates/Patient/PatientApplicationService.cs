using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Platform.Dispatching;

namespace Prototype.Domain.Aggregates.Patient
{
    public class PatientApplicationService : IMessageHandler
    {
        public void Handle(CreatePatient c)
        {
        }
    }
}