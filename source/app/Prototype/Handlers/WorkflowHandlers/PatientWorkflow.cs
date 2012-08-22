using Prototype.Domain.Aggregates.Patient.Commands;
using Prototype.Domain.Aggregates.Patient.Events;
using Prototype.Platform.Dispatching;

namespace Prototype.Handlers.WorkflowHandlers
{
    public class PatientWorkflow : IMessageHandler
    {
        private readonly ICommandBus _bus;

        public PatientWorkflow(ICommandBus bus)
        {
            _bus = bus;
        }

        public void Handle(PatientUpdated e)
        {
            if (e.Level > 100)
                _bus.Send(new DeletePatient(e.Id, "Deleted because Level was higher than 100"));
        }
    }
}