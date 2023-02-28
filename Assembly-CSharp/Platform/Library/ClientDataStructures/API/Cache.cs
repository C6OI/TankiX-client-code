namespace Platform.Library.ClientDataStructures.API {
    public interface Cache<T> : AbstractCache {
        T GetInstance();

        void Free(T item);

        void SetMaxSize(int maxSize);
    }
}