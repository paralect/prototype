using System;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using Prototype.Platform.Dispatching.Exceptions;

namespace Prototype.Platform.Dispatching
{
    public class Dispatcher : IDispatcher
    {
        /// <summary>
        /// Service Locator that is used to create handlers
        /// </summary>
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Registry of all registered handlers
        /// </summary>
        private readonly DispatcherHandlerRegistry _registry;

        /// <summary>
        /// Number of retries in case exception was logged
        /// </summary>
        private readonly int _maxRetries;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current message that is dispatched 
        /// </summary>
        [ThreadStatic]
        public static Object CurrentMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Dispatcher(DispatcherConfiguration configuration)
        {
            if (configuration.ServiceLocator == null)
                throw new ArgumentException("Unity Container is not registered for distributor.");

            if (configuration.DispatcherHandlerRegistry == null)
                throw new ArgumentException("Dispatcher Handler Registry is null in distributor.");

            _serviceLocator = configuration.ServiceLocator;
            _registry = configuration.DispatcherHandlerRegistry;
            _maxRetries = configuration.NumberOfRetries;

            // order handlers 
            _registry.InsureOrderOfHandlers();
        }

        /// <summary>
        /// Factory method
        /// </summary>
        public static Dispatcher Create(Func<DispatcherConfiguration, DispatcherConfiguration> configurationAction)
        {
            var config = new DispatcherConfiguration();
            configurationAction(config);
            return new Dispatcher(config);
        }

        public void Dispatch(Object message)
        {
            Dispatch(message, null);
        }

        public void Dispatch(Object message, Action<Exception> exceptionObserver)
        {
            try
            {
                CurrentMessage = message;

                var subscriptions = _registry.GetSubscriptions(message.GetType());

                foreach (var subscription in subscriptions)
                {
                    var handler = _serviceLocator.GetInstance(subscription.HandlerType);

                    try
                    {
                        ExecuteHandler(handler, message, exceptionObserver);
                    }
                    catch (HandlerException handlerException)
                    {
                        _log.ErrorException("Message handling failed.", handlerException);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new DispatchingException("Error when dispatching message", exception);
            }
        }

        private void ExecuteHandler(Object handler, Object message, Action<Exception> exceptionObserver = null)
        {
            var attempt = 0;
            while (attempt < _maxRetries)
            {
                try
                {
                    var context = new DispatcherInvocationContext(this, handler, message);

                    if (_registry.Interceptors.Count > 0)
                    {
                        // Call interceptors in backward order
                        for (int i = _registry.Interceptors.Count - 1; i >= 0; i--)
                        {
                            var interceptorType = _registry.Interceptors[i];
                            var interceptor = (IMessageHandlerInterceptor)_serviceLocator.GetInstance(interceptorType);
                            context = new DispatcherInterceptorContext(interceptor, context);
                        }
                    }

                    context.Invoke();

                    // message handled correctly - so that should be 
                    // the final attempt
                    attempt = _maxRetries;
                }
                catch (Exception exception)
                {
                    if (exceptionObserver != null)
                        exceptionObserver(exception);

                    attempt++;

                    if (attempt == _maxRetries)
                    {
                        throw new HandlerException(String.Format(
                            "Exception in the handler {0} for message {1}", handler.GetType().FullName, message.GetType().FullName), exception, message);

                    }
                }
            }            
        }

        public void InvokeDynamic(Object handler, Object message)
        {
            dynamic dynamicHandler = handler;
            dynamic dynamicMessage = message;

            dynamicHandler.Handle(dynamicMessage);
        }

        private readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> _methodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        public void InvokeByReflection(Object handler, Object message)
        {
            var methodDescriptor = new MethodDescriptor(handler.GetType(), message.GetType());
            MethodInfo methodInfo = null;
            if (!_methodCache.TryGetValue(methodDescriptor, out methodInfo))
                _methodCache[methodDescriptor] = methodInfo = handler.GetType().GetMethod("Handle", new[] { message.GetType() });

            if (methodInfo == null)
                return;

            methodInfo.Invoke(handler, new[] { message });
        }
    }

    public struct MethodDescriptor
    {
        public readonly Type HandlerType;
        public readonly Type MessageType;

        public MethodDescriptor(Type handlerType, Type messageType) : this()
        {
            HandlerType = handlerType;
            MessageType = messageType;
        }

        public bool Equals(MethodDescriptor descriptor)
        {
            return descriptor.HandlerType == HandlerType && descriptor.MessageType == MessageType;
        }

        public override bool Equals(object descriptor)
        {
            if (ReferenceEquals(null, descriptor))
                return false;

            if (descriptor.GetType() != typeof(MethodDescriptor))
                return false;

            return Equals((MethodDescriptor) descriptor);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType != null ? HandlerType.GetHashCode() : 0) * 397) 
                     ^ (MessageType != null ? MessageType.GetHashCode() : 0);
            }
        }
    }

}
