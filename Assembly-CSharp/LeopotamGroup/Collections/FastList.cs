using System;
using System.Collections;
using System.Collections.Generic;

namespace LeopotamGroup.Collections {
    [Serializable]
    public class FastList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable {
        const int InitCapacity = 8;

        readonly EqualityComparer<T> _comparer;

        readonly bool _isNullable;

        T[] _items;

        bool _useObjectCastComparer;

        public FastList()
            : this(null) { }

        public FastList(EqualityComparer<T> comparer)
            : this(8, comparer) { }

        public FastList(int capacity, EqualityComparer<T> comparer = null) {
            Type typeFromHandle = typeof(T);
            _isNullable = !typeFromHandle.IsValueType || Nullable.GetUnderlyingType(typeFromHandle) != null;
            Capacity = capacity <= 8 ? 8 : capacity;
            Count = 0;
            _comparer = comparer;
            _items = new T[Capacity];
        }

        public int Capacity { get; private set; }

        public int Count { get; private set; }

        public T this[int index] {
            get {
                if (index >= Count) {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[index];
            }
            set {
                if (index >= Count) {
                    throw new ArgumentOutOfRangeException();
                }

                _items[index] = value;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item) {
            if (Count == Capacity) {
                if (Capacity > 0) {
                    Capacity <<= 1;
                } else {
                    Capacity = 8;
                }

                T[] array = new T[Capacity];
                Array.Copy(_items, array, Count);
                _items = array;
            }

            _items[Count] = item;
            Count++;
        }

        public void Clear() {
            Clear(false);
        }

        public bool Contains(T item) => IndexOf(item) != -1;

        public void CopyTo(T[] array, int arrayIndex) {
            Array.Copy(_items, 0, array, arrayIndex, Count);
        }

        public int IndexOf(T item) {
            int num;

            if (_useObjectCastComparer && _isNullable) {
                num = Count - 1;

                while (num >= 0 && (object)_items[num] != (object)item) {
                    num--;
                }
            } else if (_comparer != null) {
                num = Count - 1;

                while (num >= 0 && !_comparer.Equals(_items[num], item)) {
                    num--;
                }
            } else {
                num = Array.IndexOf(_items, item, 0, Count);
            }

            return num;
        }

        public void Insert(int index, T item) {
            if (index < 0 || index > Count) {
                throw new ArgumentOutOfRangeException();
            }

            Reserve(1, false, false);
            Array.Copy(_items, index, _items, index + 1, Count - index);
            _items[index] = item;
            Count++;
        }

        public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();

        public bool Remove(T item) {
            int num = IndexOf(item);

            if (num == -1) {
                return false;
            }

            RemoveAt(num);
            return true;
        }

        public void RemoveAt(int id) {
            if (id >= 0 && id < Count) {
                Count--;
                Array.Copy(_items, id + 1, _items, id, Count - id);
            }
        }

        public void AddRange(IEnumerable<T> data) {
            if (data == null) {
                throw new ArgumentNullException("data");
            }

            ICollection<T> collection = data as ICollection<T>;

            if (collection != null) {
                int count = collection.Count;

                if (count > 0) {
                    Reserve(count, false, false);
                    collection.CopyTo(_items, Count);
                    Count += count;
                }

                return;
            }

            foreach (T datum in data) {
                Add(datum);
            }
        }

        public void AssignData(T[] data, int count) {
            if (data == null) {
                throw new ArgumentNullException("data");
            }

            _items = data;
            Count = count >= 0 ? count : 0;
            Capacity = _items.Length;
        }

        public void Clear(bool forceSetDefaultValues) {
            if (_isNullable || forceSetDefaultValues) {
                for (int num = Count - 1; num >= 0; num--) {
                    _items[num] = default;
                }
            }

            Count = 0;
        }

        public void FillWithEmpty(int amount, bool clearCollection = false, bool forceSetDefaultValues = true) {
            if (amount > 0) {
                if (clearCollection) {
                    Count = 0;
                }

                Reserve(amount, clearCollection, forceSetDefaultValues);
                Count += amount;
            }
        }

        public T[] GetData() => _items;

        public T[] GetData(out int count) {
            count = Count;
            return _items;
        }

        public bool RemoveLast(bool forceSetDefaultValues = true) {
            if (Count <= 0) {
                return false;
            }

            Count--;

            if (forceSetDefaultValues) {
                _items[Count] = default;
            }

            return true;
        }

        public void Reserve(int amount, bool totalAmount = false, bool forceSetDefaultValues = true) {
            if (amount <= 0) {
                return;
            }

            int num = !totalAmount ? Count : 0;
            int num2 = num + amount;

            if (num2 > Capacity) {
                if (Capacity <= 0) {
                    Capacity = 8;
                }

                while (Capacity < num2) {
                    Capacity <<= 1;
                }

                T[] array = new T[Capacity];
                Array.Copy(_items, array, Count);
                _items = array;
            }

            if (forceSetDefaultValues) {
                for (int i = Count; i < num2; i++) {
                    _items[i] = default;
                }
            }
        }

        public void Reverse() {
            if (Count > 0) {
                int i = 0;

                for (int num = Count >> 1; i < num; i++) {
                    T val = _items[i];
                    _items[i] = _items[Count - i - 1];
                    _items[Count - i - 1] = val;
                }
            }
        }

        public T[] ToArray() {
            T[] array = new T[Count];

            if (Count > 0) {
                Array.Copy(_items, array, Count);
            }

            return array;
        }

        public void UseCastToObjectComparer(bool state) {
            _useObjectCastComparer = state;
        }
    }
}