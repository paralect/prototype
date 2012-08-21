using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.Unity;

namespace Prototype.Platform.Unity
{
    public class SessionLifetimeManager : LifetimeManager
    {
        private readonly string _objectKey;
        private readonly string _sessionKey = "UnitySessionObjects";
        private static readonly object _lock = new object();

        public SessionLifetimeManager(string objectKey)
        {
            _objectKey = objectKey;
        }

        /// <summary>
        /// Returns null, if object not found
        /// </summary>
        public override object GetValue()
        {
            var objects = GetObjectsDictionary();

            Object obj;
            var result = objects.TryGetValue(_objectKey, out obj);

            return result ? obj : null;
        }

        public override void SetValue(object value)
        {
            var objects = GetObjectsDictionary();
            objects[_objectKey] = value;
        }

        public override void RemoveValue()
        {
            var objects = GetObjectsDictionary();
            objects.Remove(_objectKey);
        }

        private Dictionary<String, Object> GetObjectsDictionary()
        {
            if (HttpContext.Current.Session[_sessionKey] == null)
            {
                lock (_lock)
                {
                    if (HttpContext.Current.Session[_sessionKey] == null)
                    {
                        HttpContext.Current.Session[_sessionKey] = new Dictionary<String, Object>();
                    }
                }
            }

            return (Dictionary<String, Object>)HttpContext.Current.Session[_sessionKey];
        }
    }
}