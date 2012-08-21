using System;

namespace Prototype.Platform.Dispatching
{
    public interface IDispatcher
    {
        void Dispatch(Object message);
    }
}