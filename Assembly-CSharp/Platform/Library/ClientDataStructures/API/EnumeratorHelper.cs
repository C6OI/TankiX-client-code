using System;

namespace Platform.Library.ClientDataStructures.API {
    public static class EnumeratorHelper {
        public enum EnumeratorState {
            Before = 0,
            Current = 1,
            After = 2
        }

        public static void CheckVersion(int version, int actualCollectionVersion) {
            if (version != actualCollectionVersion) {
                throw new CollectionChangedException();
            }
        }

        public static void CheckCurrentState(EnumeratorState state) {
            switch (state) {
                case EnumeratorState.Before:
                    throw new EnumeratorInvalidStateException("Enumerator in initial satte. Use MoveNext() for moving to 1st element.");

                case EnumeratorState.After:
                    throw new EnumeratorInvalidStateException("End of enumerator. Use Reset().");
            }
        }

        public class CollectionChangedException : InvalidOperationException {
            public CollectionChangedException()
                : base("Collections changed while enumeration.") { }
        }

        public class EnumeratorInvalidStateException : InvalidOperationException {
            public EnumeratorInvalidStateException(string message)
                : base(message) { }
        }
    }
}