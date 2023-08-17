using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.Impl {
    public class SingletonList<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T> {
        IEnumerator<T> enumerator;
        T value;

        public SingletonList() { }

        public SingletonList(T value) => this.value = value;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => 1;

        public bool IsReadOnly => true;

        public T this[int index] {
            get {
                if (index == 0) {
                    return value;
                }

                throw new NotImplementedException();
            }
            set => throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() {
            if (enumerator == null) {
                enumerator = new SingletonEnumerator<T>(value);
            } else {
                ((SingletonEnumerator<T>)enumerator).value = value;
                enumerator.Reset();
            }

            return enumerator;
        }

        public void Add(T item) => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public bool Contains(T item) => item.Equals(value);

        public void CopyTo(T[] array, int arrayIndex) => array[arrayIndex] = value;

        public bool Remove(T item) => throw new NotImplementedException();

        public int IndexOf(T item) => throw new NotImplementedException();

        public void Insert(int index, T item) => throw new NotImplementedException();

        public void RemoveAt(int index) => throw new NotImplementedException();

        public SingletonList<T> Init(T value) {
            this.value = value;
            return this;
        }
    }
}