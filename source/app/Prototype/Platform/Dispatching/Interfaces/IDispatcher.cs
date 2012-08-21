using System;

namespace Prototype.Platform.Dispatching
{
    public interface IDispatcher
    {
        void Dispatch(Object message);
        void Dispatch(Object message, Action<Exception> exceptionObserver);
    }
}