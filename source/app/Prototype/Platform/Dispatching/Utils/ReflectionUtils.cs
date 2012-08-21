using System.Reflection;

namespace Prototype.Platform.Dispatching.Utils
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Returns attribute instance for specified type. Will return default type value if not found or not single.
        /// </summary>
        public static TAttribute GetSingleAttribute<TAttribute>(MemberInfo type)
        {
            var identities = type.GetCustomAttributes(typeof (TAttribute), false);

            if (identities.Length != 1)
                return default(TAttribute);

            if (!(identities[0] is TAttribute))
                return default(TAttribute);

            return (TAttribute) identities[0];
        }
    }
}