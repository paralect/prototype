using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Prototype.Platform.Unity
{
    public class HttpApplicationUnityContext
    {
        private const String UnityContextKey = "UnityContext";

        private static Object _lock = new Object();

        private static IUnityContainer _current;

        public static IUnityContainer Current
        {
            get
            {
                IUnityContainer unity;

                if (HttpContext.Current != null)
                    unity = HttpContext.Current.Application[UnityContextKey] as IUnityContainer;
                else
                    unity = _current;

                if (unity == null)
                {
                    lock (_lock)
                    {
                        if (unity == null)
                        {
                            unity = new UnityContainer();

                            if (HttpContext.Current != null)
                                HttpContext.Current.Application[UnityContextKey] = unity;
                            else
                                _current = unity;
                        }
                    }
                }

                return unity;
            }
        }
    }
}