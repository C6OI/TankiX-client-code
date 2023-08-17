namespace Platform.Library.ClientDataStructures.API {
    public interface CacheMultisizeArray<T> : AbstractCache {
        T[] GetInstanceArray(int length);

        void Free(T[] item);

        void FreeAll(int length);

        new void Dispose();

        void Dispose(int length);
    }
}