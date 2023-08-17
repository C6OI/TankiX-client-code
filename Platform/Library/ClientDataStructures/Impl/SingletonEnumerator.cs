using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.Impl {
    class SingletonEnumerator<T> : IEnumerator, IDisposable, IEnumerator<T> {
        bool first;
        internal T value;

        public SingletonEnumerator(T value) {
            this.value = value;
            first = true;
        }

        public void Dispose() { }

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            if (first) {
                first = false;
                return true;
            }

            return false;
        }

        public void Reset() => first = true;

        public T Current => value;
    }
}