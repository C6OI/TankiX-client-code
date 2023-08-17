using System;
using System.Collections;

namespace log4net.Util {
    [Serializable]
    public sealed class EmptyCollection : IEnumerable, ICollection {
        EmptyCollection() { }

        public static EmptyCollection Instance { get; } = new();

        public bool IsSynchronized => true;

        public int Count => 0;

        public object SyncRoot => this;

        public void CopyTo(Array array, int index) { }

        public IEnumerator GetEnumerator() => NullEnumerator.Instance;
    }
}