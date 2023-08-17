using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpCompress {
    class ReadOnlyCollection<T> : IEnumerable, ICollection<T>, IEnumerable<T> {
        readonly ICollection<T> collection;

        public ReadOnlyCollection(ICollection<T> collection) => this.collection = collection;

        public int Count => collection.Count;

        public bool IsReadOnly => true;

        public void Add(T item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(T item) => collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => collection.CopyTo(array, arrayIndex);

        public bool Remove(T item) => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}