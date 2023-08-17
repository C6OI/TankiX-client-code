using System;
using System.Collections;

namespace log4net.Util {
    public sealed class NullEnumerator : IEnumerator {
        NullEnumerator() { }

        public static NullEnumerator Instance { get; } = new();

        public object Current => throw new InvalidOperationException();

        public bool MoveNext() => false;

        public void Reset() { }
    }
}