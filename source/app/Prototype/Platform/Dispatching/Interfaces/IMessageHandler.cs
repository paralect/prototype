namespace Prototype.Platform.Dispatching
{
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }

    public interface IMessageHandler
    {
        // Marker interface
    }

}
