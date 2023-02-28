namespace Platform.Library.ClientDataStructures.API {
    public interface AbstractCache {
        void FreeAll();

        void Dispose();
    }
}