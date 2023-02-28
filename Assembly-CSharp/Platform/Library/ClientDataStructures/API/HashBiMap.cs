using System.Collections;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.Impl;
using KeyNotFoundException = Platform.Library.ClientDataStructures.Impl.KeyNotFoundException;

namespace Platform.Library.ClientDataStructures.API {
    public class HashBiMap<TKey, TValue> : IBiMap<TKey, TValue>, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable {
        readonly Dictionary<TKey, TValue> data = new();

        readonly InverseBiMap<TValue, TKey> inverse;

        public HashBiMap() => inverse = new InverseBiMap<TValue, TKey>(this);

        public int Count => data.Count;

        public IBiMap<TValue, TKey> Inverse => inverse;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => data.Keys;

        public ICollection<TValue> Values => data.Values;

        public TValue this[TKey key] {
            get {
                TValue value;

                if (data.TryGetValue(key, out value)) {
                    return value;
                }

                throw new KeyNotFoundException(key);
            }
            set => ForcePut(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value) {
            CheckNotNulls(key, value);

            if (data.ContainsKey(key)) {
                throw new KeyAlreadyExistsException(key);
            }

            if (inverse.inverseData.ContainsKey(value)) {
                throw new ValueAlreadyExistsException(value);
            }

            data.Add(key, value);
            inverse.inverseData.Add(value, key);
        }

        public void ForcePut(TKey key, TValue value) {
            CheckNotNulls(key, value);
            TValue value2;

            if (data.TryGetValue(key, out value2)) {
                data.Remove(key);
                inverse.inverseData.Remove(value2);
            }

            TKey value3;

            if (inverse.inverseData.TryGetValue(value, out value3)) {
                inverse.inverseData.Remove(value);
                data.Remove(value3);
            }

            data.Add(key, value);
            inverse.inverseData.Add(value, key);
        }

        public void Clear() {
            data.Clear();
            inverse.inverseData.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => Contains(item.Key, item.Value);

        public bool Contains(TKey key, TValue value) {
            TValue value2;

            if (data.TryGetValue(key, out value2)) {
                return Equals(value, value2);
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            ((ICollection<KeyValuePair<TKey, TValue>>)data).CopyTo(array, arrayIndex);
        }

        public bool ContainsKey(TKey key) => data.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove(TKey key) {
            TValue value;

            if (data.TryGetValue(key, out value)) {
                data.Remove(key);
                inverse.inverseData.Remove(value);
                return true;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key, item.Value);

        public bool Remove(TKey key, TValue value) {
            TValue value2;

            if (data.TryGetValue(key, out value2) && Equals(value, value2)) {
                data.Remove(key);
                inverse.inverseData.Remove(value);
                return true;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TValue value) => data.TryGetValue(key, out value);

        void CheckNotNulls(TKey key, TValue value) {
            if (key == null) {
                throw new KeyIsNullExcpetion();
            }

            if (value == null) {
                throw new ValueIsNullException();
            }
        }

        class InverseBiMap<TValue, TKey> : IBiMap<TValue, TKey>, IDictionary<TValue, TKey>, ICollection<KeyValuePair<TValue, TKey>>, IEnumerable<KeyValuePair<TValue, TKey>>, IEnumerable {
            internal readonly Dictionary<TValue, TKey> inverseData;

            public InverseBiMap(IBiMap<TKey, TValue> direct) {
                inverseData = new Dictionary<TValue, TKey>();
                Inverse = direct;
            }

            public int Count => Inverse.Count;

            public IBiMap<TKey, TValue> Inverse { get; }

            public bool IsReadOnly => false;

            public TKey this[TValue _value] {
                get {
                    TKey value;

                    if (inverseData.TryGetValue(_value, out value)) {
                        return value;
                    }

                    throw new ValueNotFoundException(_value);
                }
                set => Inverse.ForcePut(value, _value);
            }

            public ICollection<TValue> Keys => inverseData.Keys;

            public ICollection<TKey> Values => inverseData.Values;

            public IEnumerator<KeyValuePair<TValue, TKey>> GetEnumerator() => inverseData.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public void Add(KeyValuePair<TValue, TKey> item) {
                Inverse.Add(item.Value, item.Key);
            }

            public void Clear() {
                Inverse.Clear();
            }

            public bool Contains(KeyValuePair<TValue, TKey> item) => Inverse.Contains(item.Value, item.Key);

            public bool Contains(TValue value, TKey key) => Inverse.Contains(key, value);

            public void CopyTo(KeyValuePair<TValue, TKey>[] array, int arrayIndex) {
                ((ICollection<KeyValuePair<TValue, TKey>>)inverseData).CopyTo(array, arrayIndex);
            }

            public bool ContainsKey(TValue value) => inverseData.ContainsKey(value);

            public void Add(TValue value, TKey key) {
                Inverse.Add(key, value);
            }

            public bool Remove(TValue value) {
                TKey value2;

                if (inverseData.TryGetValue(value, out value2)) {
                    return Inverse.Remove(value2);
                }

                return false;
            }

            public bool Remove(KeyValuePair<TValue, TKey> item) => Inverse.Remove(item.Value, item.Key);

            public bool Remove(TValue value, TKey key) => Inverse.Remove(key, value);

            public bool TryGetValue(TValue value, out TKey key) => inverseData.TryGetValue(value, out key);

            public void ForcePut(TValue value, TKey key) {
                Inverse.ForcePut(key, value);
            }
        }
    }
}