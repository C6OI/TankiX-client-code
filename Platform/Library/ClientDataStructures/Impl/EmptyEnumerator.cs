using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.Impl {
    public class EmptyEnumerator<T> : IEnumerator, IDisposable, IEnumerator<T> {
        public static IEnumerator<T> Instance = new EmptyEnumerator<T>();

        public void Dispose() { }

        object IEnumerator.Current => Current;

        public bool MoveNext() => false;

        public void Reset() { }

        public T Current => throw new NotSupportedException();
    }
}