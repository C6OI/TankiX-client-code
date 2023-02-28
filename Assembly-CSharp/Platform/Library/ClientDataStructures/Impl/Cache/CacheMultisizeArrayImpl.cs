using System;
using System.Collections.Generic;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Library.ClientDataStructures.Impl.Cache {
    public class CacheMultisizeArrayImpl<T> : CacheMultisizeArray<T>, AbstractCache {
        readonly Action<T[]> cleaner;

        readonly Dictionary<int, CacheImpl<T[]>> elementsCountToArrayCache = new();

        public CacheMultisizeArrayImpl() { }

        public CacheMultisizeArrayImpl(Action<T[]> cleaner) => this.cleaner = cleaner;

        public int CacheArrayCount => elementsCountToArrayCache.Count;

        public T[] GetInstanceArray(int length) {
            CacheImpl<T[]> value;

            if (!elementsCountToArrayCache.TryGetValue(length, out value)) {
                value = new CacheImpl<T[]>(cleaner, length);
                elementsCountToArrayCache.Add(length, value);
            }

            return value.GetInstance();
        }

        public void Free(T[] item) {
            int key = item.Length;
            CacheImpl<T[]> value;

            if (elementsCountToArrayCache.TryGetValue(key, out value)) {
                value.Free(item);
            }
        }

        public void FreeAll(int length) {
            CacheImpl<T[]> value;

            if (elementsCountToArrayCache.TryGetValue(length, out value)) {
                value.FreeAll();
            }
        }

        public void Dispose() {
            elementsCountToArrayCache.Clear();
        }

        public void Dispose(int length) {
            elementsCountToArrayCache.Remove(length);
        }

        public void FreeAll() {
            elementsCountToArrayCache.ForEach(delegate(KeyValuePair<int, CacheImpl<T[]>> kv) {
                kv.Value.FreeAll();
            });
        }

        public int GetArrayInstancesCount(int length) {
            CacheImpl<T[]> value;

            if (!elementsCountToArrayCache.TryGetValue(length, out value)) {
                throw new NoCachesForArrayWithLengthException(length);
            }

            return value.InstanceCount;
        }

        public int GetFreeCountInCache(int length) {
            CacheImpl<T[]> value;

            if (!elementsCountToArrayCache.TryGetValue(length, out value)) {
                throw new NoCachesForArrayWithLengthException(length);
            }

            return value.FreeCount;
        }

        public int GetCapacityInCache(int length) {
            CacheImpl<T[]> value;

            if (!elementsCountToArrayCache.TryGetValue(length, out value)) {
                throw new NoCachesForArrayWithLengthException(length);
            }

            return value.Capacity;
        }

        public object GetObjectInstance() => throw new NotImplementedException();

        public void FreeObjectInstance(object item) {
            throw new NotImplementedException();
        }

        public class NoCachesForArrayWithLengthException : Exception {
            const string EXCEPTION_FORMAT = "Length = {0}";

            public NoCachesForArrayWithLengthException(int length)
                : base(string.Format("Length = {0}", length)) { }
        }
    }
}