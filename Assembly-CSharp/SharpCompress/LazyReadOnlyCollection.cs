using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpCompress {
    internal class LazyReadOnlyCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable {
        readonly List<T> backing = new();

        readonly IEnumerator<T> source;

        bool fullyLoaded;

        public LazyReadOnlyCollection(IEnumerable<T> source) => this.source = source.GetEnumerator();

        public int Count {
            get {
                EnsureFullyLoaded();
                return backing.Count;
            }
        }

        public bool IsReadOnly => true;

        public void Add(T item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(T item) {
            EnsureFullyLoaded();
            return backing.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            EnsureFullyLoaded();
            backing.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) => throw new NotImplementedException();

        public IEnumerator<T> GetEnumerator() => new LazyLoader(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void EnsureFullyLoaded() {
            if (!fullyLoaded) {
                this.ForEach(delegate { });
                fullyLoaded = true;
            }
        }

        internal IEnumerable<T> GetLoaded() => backing;

        class LazyLoader : IEnumerator<T>, IDisposable, IEnumerator {
            readonly LazyReadOnlyCollection<T> lazyReadOnlyCollection;

            bool disposed;

            int index = -1;

            internal LazyLoader(LazyReadOnlyCollection<T> lazyReadOnlyCollection) => this.lazyReadOnlyCollection = lazyReadOnlyCollection;

            object IEnumerator.Current => Current;

            public T Current => lazyReadOnlyCollection.backing[index];

            public void Dispose() {
                if (!disposed) {
                    disposed = true;
                }
            }

            public bool MoveNext() {
                if (index + 1 < lazyReadOnlyCollection.backing.Count) {
                    index++;
                    return true;
                }

                if (!lazyReadOnlyCollection.fullyLoaded && lazyReadOnlyCollection.source.MoveNext()) {
                    lazyReadOnlyCollection.backing.Add(lazyReadOnlyCollection.source.Current);
                    index++;
                    return true;
                }

                lazyReadOnlyCollection.fullyLoaded = true;
                return false;
            }

            public void Reset() {
                throw new NotImplementedException();
            }
        }
    }
}