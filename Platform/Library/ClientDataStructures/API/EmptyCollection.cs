using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Platform.Library.ClientDataStructures.API {
    public class EmptyCollection<T> : IEnumerable, ICollection, ICollection<T>, IEnumerable<T> {
        public object SyncRoot => this;

        public bool IsSynchronized => true;

        public int Count => 0;

        public void CopyTo(Array array, int index) { }

        public bool IsReadOnly => true;

        public void Add(T item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(T item) => false;

        public void CopyTo(T[] array, int arrayIndex) { }

        public bool Remove(T item) => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => default(EmptyEnumerator);

        IEnumerator IEnumerable.GetEnumerator() => default(EmptyEnumerator);

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct EmptyEnumerator : IEnumerator, IDisposable, IEnumerator<T> {
            object IEnumerator.Current => throw new NotImplementedException();

            public T Current => throw new NotImplementedException();

            public void Dispose() { }

            public bool MoveNext() => false;

            public void Reset() { }
        }
    }
}