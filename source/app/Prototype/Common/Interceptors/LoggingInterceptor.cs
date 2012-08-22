using System;
using Microsoft.Practices.Unity;
using Prototype.Platform.Dispatching;
using Prototype.Platform.Domain;
using Prototype.Platform.Logging;

namespace Prototype.Common.Interceptors
{
    /// <summary>
    /// This interceptors simply intercept every invocation of handler 
    /// in order to write logs about commands and events
    /// </summary>
    public class LoggingInterceptor : IMessageHandlerInterceptor
    {
        [Dependency]
        public LogManager LogManager { get; set; }

        public void Intercept(DispatcherInvocationContext context)
        {
            if (context.Message == null)
                return;

            var command = context.Message as ICommand;

            if (command != null)
            {
                try
                {
                    LogManager.LogCommand(command);
                    context.Invoke();
                    LogManager.LogCommandHandler(command.Metadata.CommandId, context.Handler.GetType().FullName);
                }
                catch(Exception ex)
                {
                    LogManager.LogCommandHandler(command.Metadata.CommandId, context.Handler.GetType().FullName, ex);
                    throw;
                }

                return;
            }

            var evnt = context.Message as IEvent;

            if (evnt != null)
            {
                try
                {
                    LogManager.LogEvent(evnt);
                    context.Invoke();
                    LogManager.LogEventHandler(evnt.Metadata.CommandId, evnt.Metadata.EventId, context.Handler.GetType().FullName);
                }
                catch (Exception ex)
                {
                    LogManager.LogEventHandler(evnt.Metadata.CommandId, evnt.Metadata.EventId, context.Handler.GetType().FullName, ex);
                    throw;
                }

                return;
            }

            // if this is not command or event - execute as usual
            context.Invoke();
        }
    }
}