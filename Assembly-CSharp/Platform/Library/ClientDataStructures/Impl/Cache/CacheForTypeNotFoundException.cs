using System;

namespace Platform.Library.ClientDataStructures.Impl.Cache {
    public class CacheForTypeNotFoundException : Exception {
        public CacheForTypeNotFoundException(Type type)
            : base("Type: " + type) { }
    }
}