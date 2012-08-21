using System;
using Microsoft.Practices.Unity;

namespace Prototype.Platform.Unity
{
    public class AppDomainUnityContext
    {
        private static Object _lock = new Object();

        private static IUnityContainer _current;

        public static IUnityContainer Current
        {
            get
            {
                IUnityContainer unity = _current;

                if (unity == null)
                {
                    lock (_lock)
                    {
                        if (unity == null)
                        {
                            unity = new UnityContainer();

                            _current = unity;
                        }
                    }
                }

                return unity;
            }
        }
    }
}