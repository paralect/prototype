using System;
using System.Web;
using Microsoft.Practices.Unity;

namespace Prototype.Platform.Unity
{
    public class RequestLifetimeManager : LifetimeManager
    {
        private readonly string _key = "UnityRequestObject-" + Guid.NewGuid().ToString();

        public override object GetValue()
        {
            return HttpContext.Current.Items[_key];
        }
        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(_key);
        }
        public override void SetValue(object value)
        {
            HttpContext.Current.Items[_key] = value;
        }
    }
}