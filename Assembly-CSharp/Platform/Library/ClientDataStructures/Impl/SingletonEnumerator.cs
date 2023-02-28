using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.Impl {
    internal class SingletonEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator {
        bool first;
        internal T value;

        public SingletonEnumerator(T value) {
            this.value = value;
            first = true;
        }

        object IEnumerator.Current => Current;

        public T Current => value;

        public void Dispose() { }

        public bool MoveNext() {
            if (first) {
                first = false;
                return true;
            }

            return false;
        }

        public void Reset() {
            first = true;
        }
    }
}