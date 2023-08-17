using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public class Pair<K, V> {
        public Pair(K k, V v) {
            Key = k;
            Value = v;
        }

        public V Value { get; set; }

        public K Key { get; set; }

        protected bool Equals(Pair<K, V> other) => EqualityComparer<V>.Default.Equals(Value, other.Value) &&
                                                   EqualityComparer<K>.Default.Equals(Key, other.Key);

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != GetType()) {
                return false;
            }

            return Equals((Pair<K, V>)obj);
        }

        public override int GetHashCode() => EqualityComparer<V>.Default.GetHashCode(Value) * 397 ^
                                             EqualityComparer<K>.Default.GetHashCode(Key);
    }
}