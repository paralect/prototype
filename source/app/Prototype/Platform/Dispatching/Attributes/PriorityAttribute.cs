using System;

namespace Prototype.Platform.Dispatching.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; set; }

        public PriorityAttribute(Int32 priority)
        {
            Priority = priority;
        }
    }
}