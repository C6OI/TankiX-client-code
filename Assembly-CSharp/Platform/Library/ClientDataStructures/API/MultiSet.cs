using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public class MultiSet<KEY> : ICollection<KEY>, IEnumerable<KEY>, IEnumerable {
        readonly Dictionary<KEY, int?> _values = new();

        public int Count => _values.Count;

        public bool IsReadOnly => false;

        public void Add(KEY item) {
            Add(item, 1);
        }

        public void Clear() {
            _values.Clear();
        }

        public bool Contains(KEY item) => _values.ContainsKey(item);

        public bool Remove(KEY item) {
            if (!_values.ContainsKey(item)) {
                return false;
            }

            int? num = _values[item];

            if (num == 1) {
                _values.Remove(item);
                return true;
            }

            _values[item] = num - 1;
            return true;
        }

        public IEnumerator<KEY> GetEnumerator() => _values.Keys.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(KEY[] array, int arrayIndex) {
            foreach (KeyValuePair<KEY, int?> value in _values) {
                array[arrayIndex++] = value.Key;
            }
        }

        public void Add(KEY item, int count) {
            if (_values.ContainsKey(item)) {
                _values[item] += count;
            } else {
                _values[item] = count;
            }
        }

        public int Occurrence(KEY key) => _values.ContainsKey(key) ? _values[key].Value : 0;

        public void AddAll(ICollection<KEY> collection) {
            MultiSet<KEY> multiSet = collection as MultiSet<KEY>;

            if (multiSet != null) {
                foreach (KEY item in multiSet) {
                    Add(item, multiSet.Occurrence(item));
                }

                return;
            }

            foreach (KEY item2 in collection) {
                Add(item2);
            }
        }

        public void Remove(MultiSet<KEY> multiSet) {
            foreach (KEY item in multiSet) {
                if (_values.ContainsKey(item)) {
                    int num = _values[item].Value - multiSet.Occurrence(item);

                    if (num > 0) {
                        _values[item] = num;
                    } else {
                        _values.Remove(item);
                    }
                }
            }
        }
    }
}