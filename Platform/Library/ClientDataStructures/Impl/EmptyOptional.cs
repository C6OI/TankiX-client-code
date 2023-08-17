using System;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Library.ClientDataStructures.Impl {
    public class EmptyOptional<T> : Optional<T> {
        public static readonly EmptyOptional<T> Instance = new();

        public override bool IsPresent() => false;

        public override T Get() => throw new NotSupportedException();
    }
}