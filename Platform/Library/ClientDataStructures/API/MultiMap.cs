using System.Collections.Generic;
using System.Linq;

namespace Platform.Library.ClientDataStructures.API {
    public class MultiMap<TKey, TValue> : Dictionary<TKey, HashSet<TValue>> {
        public void Add(TKey key, TValue value) {
            HashSet<TValue> value2 = null;

            if (!TryGetValue(key, out value2)) {
                value2 = new HashSet<TValue>();
                Add(key, value2);
            }

            value2.Add(value);
        }

        public TKey GetKeyByValue(TValue value) => Keys.Where(x => this[x].Contains(value)).First();

        public bool ContainsValue(TValue value) => Keys.Any(x => this[x].Contains(value));

        public bool ContainsValue(TKey key, TValue value) => ContainsKey(key) && this[key].Contains(value);

        public void Remove(TKey key, TValue value) {
            if (ContainsKey(key)) {
                this[key].Remove(value);

                if (this[key].Count == 0) {
                    Remove(key);
                }
            }
        }

        public void Merge(MultiMap<TKey, TValue> toMergeWith) {
            if (toMergeWith == null) {
                return;
            }

            Enumerator enumerator = toMergeWith.GetEnumerator();

            while (enumerator.MoveNext()) {
                KeyValuePair<TKey, HashSet<TValue>> current = enumerator.Current;
                HashSet<TValue>.Enumerator enumerator2 = current.Value.GetEnumerator();
                TKey key = current.Key;

                while (enumerator2.MoveNext()) {
                    Add(key, enumerator2.Current);
                }
            }
        }

        public HashSet<TValue> GetValues(TKey key) {
            HashSet<TValue> value;
            TryGetValue(key, out value);
            return value ?? new HashSet<TValue>();
        }

        public HashSet<TValue> GetAllValues() => new(Keys.SelectMany(x => this[x]));
    }
}