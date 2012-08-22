using Prototype.Domain.Aggregates.Subject.Commands;
using Prototype.Domain.Aggregates.Subject.Events;
using Prototype.Platform.Dispatching;

namespace Prototype.Handlers.WorkflowHandlers
{
    public class SubjectWorkflow : IMessageHandler
    {
        private readonly ICommandBus _bus;

        public SubjectWorkflow(ICommandBus bus)
        {
            _bus = bus;
        }

        public void Handle(SubjectUpdated e)
        {
            if (e.Level > 100)
                _bus.Send(new DeleteSubject(e.Id, "Deleted because Level was higher than 100"));
        }
    }
}