using System;

namespace Prototype.Platform.Dispatching
{
    public class Subscription
    {
        public Type HandlerType { get; set; }
        public Int32 Priority { get; set; }

        public Subscription(Type handlerType, int priority)
        {
            HandlerType = handlerType;
            Priority = priority;
        }

        public bool Equals(Subscription other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return other.HandlerType == HandlerType && other.Priority == Priority;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof (Subscription))
                return false;

            return Equals((Subscription) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HandlerType != null ? HandlerType.GetHashCode() : 0)*397) ^ Priority;
            }
        }
    }
}