using System;
using Platform.Library.ClientDataStructures.Impl;

namespace Platform.Library.ClientDataStructures.API {
    public class Optional<T> : NonGenericOptional {
        public static Optional<T> nullableOf(T value) {
            if (value == null) {
                return empty();
            }

            return of(value);
        }

        public static Optional<T> empty() => EmptyOptional<T>.Instance;

        public static Optional<T> of(T value) => new NonEmptyOptional<T>(value);

        public virtual T Get() => throw new NotImplementedException();

        public virtual bool IsPresent() => throw new NotImplementedException();
    }
}