using System;

namespace Platform.Library.ClientResources.API {
    public class ResourceNotInStorageException : Exception {
        public ResourceNotInStorageException(string assetGuid)
            : base("Guid: " + assetGuid) { }
    }
}