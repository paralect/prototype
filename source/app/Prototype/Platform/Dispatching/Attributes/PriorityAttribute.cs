using System;

namespace Prototype.Platform.Dispatching.Attributes
{
    /// <summary>
    /// Priority defines orders of handlers execution.
    /// Handlers that have the same priority are supposed to run in parallel
    /// </summary>
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