using Platform.Library.ClientDataStructures.API;

namespace Platform.Library.ClientDataStructures.Impl {
    public class NonEmptyOptional<T> : Optional<T> {
        readonly T _value;

        public NonEmptyOptional(T value) => _value = value;

        public override T Get() => _value;

        public override bool IsPresent() => true;
    }
}