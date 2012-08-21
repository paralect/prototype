namespace Prototype.Platform.Dispatching
{
    public interface IMessageHandlerInterceptor
    {
        void Intercept(DispatcherInvocationContext context);
    }
}
