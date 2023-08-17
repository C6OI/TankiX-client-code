using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.Impl {
    public class EmptyList<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T> {
        public static IList<T> Instance = new EmptyList<T>();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => 0;

        public bool IsReadOnly => true;

        public T this[int index] {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => EmptyEnumerator<T>.Instance;

        public void Add(T item) => throw new NotImplementedException();

        public void Clear() { }

        public bool Contains(T item) => false;

        public void CopyTo(T[] array, int arrayIndex) { }

        public bool Remove(T item) => false;

        public int IndexOf(T item) => -1;

        public void Insert(int index, T item) => throw new NotImplementedException();

        public void RemoveAt(int index) => throw new NotImplementedException();

        public override int GetHashCode() => 0;
    }
}