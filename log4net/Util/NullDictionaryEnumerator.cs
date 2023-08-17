using System;
using System.Collections;

namespace log4net.Util {
    public sealed class NullDictionaryEnumerator : IEnumerator, IDictionaryEnumerator {
        NullDictionaryEnumerator() { }

        public static NullDictionaryEnumerator Instance { get; } = new();

        public object Key => throw new InvalidOperationException();

        public object Value => throw new InvalidOperationException();

        public DictionaryEntry Entry => throw new InvalidOperationException();

        public object Current => throw new InvalidOperationException();

        public bool MoveNext() => false;

        public void Reset() { }
    }
}