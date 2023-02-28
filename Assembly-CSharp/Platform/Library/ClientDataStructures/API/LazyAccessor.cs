using System;

namespace Platform.Library.ClientDataStructures.API {
    public class LazyAccessor<T> {
        readonly Func<T> initializer;
        T value;

        public LazyAccessor(Func<T> initializer) => this.initializer = initializer;

        public LazyAccessor(T value) => this.value = value;

        public T Value {
            get {
                if (value == null) {
                    value = initializer();
                }

                return value;
            }
        }
    }
}