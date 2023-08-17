using System;

namespace MIConvexHull {
    class SimpleList<T> {
        int capacity;

        public int Count;
        T[] items;

        public T this[int i] {
            get => items[i];
            set => items[i] = value;
        }

        void EnsureCapacity() {
            if (capacity == 0) {
                capacity = 32;
                items = new T[32];
                return;
            }

            T[] destinationArray = new T[capacity * 2];
            Array.Copy(items, destinationArray, capacity);
            capacity = 2 * capacity;
            items = destinationArray;
        }

        public void Add(T item) {
            if (Count + 1 > capacity) {
                EnsureCapacity();
            }

            items[Count++] = item;
        }

        public void Push(T item) {
            if (Count + 1 > capacity) {
                EnsureCapacity();
            }

            items[Count++] = item;
        }

        public T Pop() => items[--Count];

        public void Clear() => Count = 0;
    }
}