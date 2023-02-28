using System;
using System.Collections.Generic;

namespace LeopotamGroup.Collections {
    public class FastStack<T> {
        const int InitCapacity = 8;

        readonly EqualityComparer<T> _comparer;

        readonly bool _isNullable;

        int _capacity;

        T[] _items;

        bool _useObjectCastComparer;

        public FastStack()
            : this(null) { }

        public FastStack(EqualityComparer<T> comparer)
            : this(8, comparer) { }

        public FastStack(int capacity, EqualityComparer<T> comparer = null) {
            Type typeFromHandle = typeof(T);
            _isNullable = !typeFromHandle.IsValueType || Nullable.GetUnderlyingType(typeFromHandle) != null;
            _capacity = capacity <= 8 ? 8 : capacity;
            Count = 0;
            _comparer = comparer;
            _items = new T[_capacity];
        }

        public int Count { get; private set; }

        public void Clear() {
            if (_isNullable) {
                for (int num = Count - 1; num >= 0; num--) {
                    _items[num] = default;
                }
            }

            Count = 0;
        }

        public bool Contains(T item) {
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

            return num != -1;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            Array.Copy(_items, 0, array, arrayIndex, Count);
        }

        public T Peek() {
            if (Count == 0) {
                throw new IndexOutOfRangeException();
            }

            return _items[Count - 1];
        }

        public T Pop() {
            if (Count == 0) {
                throw new IndexOutOfRangeException();
            }

            Count--;
            T result = _items[Count];

            if (_isNullable) {
                _items[Count] = default;
            }

            return result;
        }

        public void Push(T item) {
            if (Count == _capacity) {
                if (_capacity > 0) {
                    _capacity <<= 1;
                } else {
                    _capacity = 8;
                }

                T[] array = new T[_capacity];
                Array.Copy(_items, array, Count);
                _items = array;
            }

            _items[Count] = item;
            Count++;
        }

        public T[] ToArray() {
            T[] array = new T[Count];

            if (Count > 0) {
                Array.Copy(_items, array, Count);
            }

            return array;
        }

        public void TrimExcess() {
            throw new NotSupportedException();
        }

        public void UseCastToObjectComparer(bool state) {
            _useObjectCastComparer = state;
        }
    }
}