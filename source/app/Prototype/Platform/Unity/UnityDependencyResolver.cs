using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Prototype.Platform.Unity
{
    /// <summary>
    /// Implementation of MVC 3 IDependencyResolver for MS Unity 2.0
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Unity container
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initialization
        /// </summary>
        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// "Implementers should return null when the service cannot be found. 
        /// Use GetServices(Type) to return multiple services."
        /// (C) http://msdn.microsoft.com/en-us/library/system.web.mvc.idependencyresolver.getservice(v=VS.98).aspx
        /// 
        /// We can check for availability of such service in container, but i don't doing it 
        /// to simplify configuration for container, views, models, actions, filters etc... 
        /// Simply speaking we are always using Unity to instantiate new object.
        /// </summary>
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// "Implementers should return an empty collection when no service can be found. 
        /// The ASP.NET MVC framework will generate a run-time error when implementations 
        /// throw an exception or return null."
        /// (C) http://msdn.microsoft.com/en-us/library/system.web.mvc.idependencyresolver.getservices(v=VS.98).aspx
        /// 
        /// We can check for availability of such services in container, but i don't doing it 
        /// to simplify configuration for container, views, models, actions, filters etc... 
        /// Simply speaking we are always using Unity to instantiate new object.
        /// </summary>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

    }
}