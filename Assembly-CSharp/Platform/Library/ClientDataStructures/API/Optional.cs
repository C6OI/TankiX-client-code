namespace Platform.Library.ClientDataStructures.API {
    public struct Optional<T> where T : class {
        public static readonly Optional<T> EMPTY = default;

        readonly T value;

        public Optional(T value) => this.value = value;

        public T Get() => value;

        public bool IsPresent() => value != null;

        public override string ToString() => string.Concat("Optional[", value, "]");

        public static Optional<T> nullableOf(T value) {
            if (value == null) {
                return empty();
            }

            return of(value);
        }

        public static Optional<T> empty() => EMPTY;

        public static Optional<T> of(T value) => new(value);
    }
}