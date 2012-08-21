using System.Collections;
using System.Collections.Generic;

namespace Prototype.Platform.Utilities
{
    public interface IIndexedEnumerable<out T> : IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }

    public static class IndexedEnumerableExtension
    {
        public static IIndexedEnumerable<T> AsIndexedEnumerable<T>(this IList<T> tail)
        {
            return new IIndexedEnumerableWrapper<T>(tail);
        }

        private class IIndexedEnumerableWrapper<T> : IIndexedEnumerable<T>
        {
            private readonly IList<T> _list;

            public int Count { get { return _list.Count; } }
            public T this[int index] { get { return _list[index]; } }

            public IIndexedEnumerableWrapper(IList<T> list)
            {
                _list = list;
            }

            public IEnumerator<T> GetEnumerator() { return _list.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return _list.GetEnumerator(); }
        }
    }
}