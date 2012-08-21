using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Prototype.Platform.Dispatching
{
    public class DispatcherConfiguration
    {
        public DispatcherHandlerRegistry DispatcherHandlerRegistry { get; set; }
        public int NumberOfRetries { get; set; }
        public IServiceLocator ServiceLocator { get; set; }
        public Type MessageHandlerMarkerInterface { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public DispatcherConfiguration()
        {
            DispatcherHandlerRegistry = new DispatcherHandlerRegistry();
            NumberOfRetries = 1;
        }
    }
}