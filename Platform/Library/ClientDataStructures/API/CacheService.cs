using System;

namespace Platform.Library.ClientDataStructures.API {
    public interface CacheService {
        Cache<T> GetCache<T>();

        Cache<T> GetCache<T>(T o);

        Cache<T> RegisterTypeCache<T>(Action<T> cleaner);

        Cache<T> RegisterTypeCache<T>();

        void Dispose();
    }
}