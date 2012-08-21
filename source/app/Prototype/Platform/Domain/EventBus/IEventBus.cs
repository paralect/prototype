using System.Collections.Generic;

namespace Prototype.Platform.Domain.EventBus
{
    public interface IEventBus
    {
        void Publish(IEvent eventMessage);
        void Publish(IEnumerable<IEvent> eventMessages);
    }
}