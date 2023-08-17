using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Platform.Library.ClientDataStructures.API {
    public class HashMultiMap<TKey, TValue> : IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IMultiMap<TKey, TValue>,
        IEnumerable<KeyValuePair<TKey, TValue>> {
        readonly Dictionary<TKey, ICollection<TValue>> data = new();

        int version;

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => Clear();

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        public bool Contains(KeyValuePair<TKey, TValue> item) => Contains(item.Key, item.Value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => Entries().CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key, item.Value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public ICollection<TValue> this[TKey key] {
            get {
                ICollection<TValue> value;
                object result;

                if (data.TryGetValue(key, out value)) {
                    ICollection<TValue> collection = value;
                    result = collection;
                } else {
                    result = Collections.EmptyList<TValue>();
                }

                return (ICollection<TValue>)result;
            }
            set => data[key] = value;
        }

        public ICollection<TKey> Keys => data.Keys;

        public ICollection<TValue> Values {
            get {
                List<TValue> list = new();

                foreach (ICollection<TValue> value in data.Values) {
                    list.AddRange(value);
                }

                return new ReadOnlyCollection<TValue>(list);
            }
        }

        public bool Contains(TKey key, TValue value) {
            ICollection<TValue> value2;

            if (data.TryGetValue(key, out value2)) {
                return value2.Contains(value);
            }

            return false;
        }

        public bool ContainsKey(TKey key) => data.ContainsKey(key);

        public bool ContainsValue(TValue value) {
            foreach (TKey key in data.Keys) {
                if (data[key].Contains(value)) {
                    return true;
                }
            }

            return false;
        }

        public ICollection<KeyValuePair<TKey, TValue>> Entries() {
            List<KeyValuePair<TKey, TValue>> list = new(Count);

            foreach (KeyValuePair<TKey, ICollection<TValue>> datum in data) {
                foreach (TValue item in datum.Value) {
                    list.Add(new KeyValuePair<TKey, TValue>(datum.Key, item));
                }
            }

            return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(list);
        }

        public void Add(TKey key, TValue value) {
            ICollection<TValue> value2;

            if (!data.TryGetValue(key, out value2)) {
                value2 = CreateValueCollection();
                data.Add(key, value2);
            }

            value2.Add(value);
            Count++;
            version++;
        }

        public void AddAll(TKey key, ICollection<TValue> values) {
            ICollection<TValue> value;

            if (!data.TryGetValue(key, out value)) {
                value = CreateValueCollection();
                data.Add(key, value);
            }

            foreach (TValue value2 in values) {
                value.Add(value2);
                Count++;
            }

            version++;
        }

        public bool Remove(TKey key, TValue value) {
            ICollection<TValue> value2;

            if (!data.TryGetValue(key, out value2)) {
                return false;
            }

            bool flag = value2.Remove(value);

            if (flag) {
                if (value2.Count == 0) {
                    data.Remove(key);
                }

                Count--;
                version++;
            }

            return flag;
        }

        public ICollection<TValue> RemoveAll(TKey key) {
            ICollection<TValue> value;

            if (!data.TryGetValue(key, out value)) {
                return new EmptyCollection<TValue>();
            }

            Count -= value.Count;
            data.Remove(key);
            version++;
            return value;
        }

        public void Clear() {
            data.Clear();
            Count = 0;
            version++;
        }

        protected virtual ICollection<TValue> CreateValueCollection() => new List<TValue>();

        struct Enumerator : IEnumerator, IDisposable, IEnumerator<KeyValuePair<TKey, TValue>> {
            readonly HashMultiMap<TKey, TValue> multimap;

            IEnumerator<TKey> keyEnumerator;

            IEnumerator<TValue> valueEnumerator;

            readonly int ver;

            EnumeratorHelper.EnumeratorState state;

            object IEnumerator.Current => Current;

            public KeyValuePair<TKey, TValue> Current {
                get {
                    EnumeratorHelper.CheckCurrentState(state);
                    return new KeyValuePair<TKey, TValue>(keyEnumerator.Current, valueEnumerator.Current);
                }
            }

            public Enumerator(HashMultiMap<TKey, TValue> multimap) {
                this.multimap = multimap;
                ver = multimap.version;
                state = EnumeratorHelper.EnumeratorState.Before;
                keyEnumerator = null;
                valueEnumerator = null;
            }

            public bool MoveNext() {
                EnumeratorHelper.CheckVersion(ver, multimap.version);

                switch (state) {
                    case EnumeratorHelper.EnumeratorState.Before: {
                        keyEnumerator = multimap.data.Keys.GetEnumerator();
                        bool flag = keyEnumerator.MoveNext();

                        if (flag) {
                            valueEnumerator = multimap.data[keyEnumerator.Current].GetEnumerator();
                            valueEnumerator.MoveNext();
                            state = EnumeratorHelper.EnumeratorState.Current;
                        } else {
                            state = EnumeratorHelper.EnumeratorState.After;
                        }

                        return flag;
                    }

                    case EnumeratorHelper.EnumeratorState.After:
                        return false;

                    default: {
                        bool flag = valueEnumerator.MoveNext();

                        if (!flag) {
                            flag = keyEnumerator.MoveNext();

                            if (flag) {
                                valueEnumerator = multimap.data[keyEnumerator.Current].GetEnumerator();
                                valueEnumerator.MoveNext();
                            }
                        }

                        if (!flag) {
                            state = EnumeratorHelper.EnumeratorState.After;
                        }

                        return flag;
                    }
                }
            }

            public void Reset() => state = EnumeratorHelper.EnumeratorState.Before;

            public void Dispose() { }
        }
    }
}