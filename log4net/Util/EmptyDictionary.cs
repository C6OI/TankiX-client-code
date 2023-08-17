using System;
using System.Collections;

namespace log4net.Util {
    [Serializable]
    public sealed class EmptyDictionary : IEnumerable, ICollection, IDictionary {
        EmptyDictionary() { }

        public static EmptyDictionary Instance { get; } = new();

        public bool IsSynchronized => true;

        public int Count => 0;

        public object SyncRoot => this;

        public void CopyTo(Array array, int index) { }

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public ICollection Keys => EmptyCollection.Instance;

        public ICollection Values => EmptyCollection.Instance;

        public object this[object key] {
            get => null;
            set => throw new InvalidOperationException();
        }

        public void Add(object key, object value) => throw new InvalidOperationException();

        public void Clear() => throw new InvalidOperationException();

        public bool Contains(object key) => false;

        public IDictionaryEnumerator GetEnumerator() => NullDictionaryEnumerator.Instance;

        public void Remove(object key) => throw new InvalidOperationException();

        IEnumerator IEnumerable.GetEnumerator() => NullEnumerator.Instance;
    }
}