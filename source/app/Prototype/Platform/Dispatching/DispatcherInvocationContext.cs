using System;

namespace Prototype.Platform.Dispatching
{
    public class DispatcherInvocationContext
    {
        private readonly Dispatching.Dispatcher _dispatcher;
        private readonly object _handler;
        private readonly object _message;

        public object Message
        {
            get { return _message; }
        }

        public object Handler
        {
            get { return _handler; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DispatcherInvocationContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DispatcherInvocationContext(Dispatching.Dispatcher dispatcher, Object handler, Object message)
        {
            _dispatcher = dispatcher;
            _handler = handler;
            _message = message;
        }

        public virtual void Invoke()
        {
            _dispatcher.InvokeByReflection(_handler, _message);
        }
    }

    public class DispatcherInterceptorContext : DispatcherInvocationContext
    {
        private readonly IMessageHandlerInterceptor _interceptor;
        private readonly DispatcherInvocationContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DispatcherInterceptorContext(IMessageHandlerInterceptor interceptor, DispatcherInvocationContext context)
        {
            _interceptor = interceptor;
            _context = context;
        }

        public override void Invoke()
        {
            _interceptor.Intercept(_context);
        }
    }
}