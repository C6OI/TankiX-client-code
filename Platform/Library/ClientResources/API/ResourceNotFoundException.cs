using System;

namespace Platform.Library.ClientResources.API {
    public class ResourceNotFoundException : Exception {
        public ResourceNotFoundException(string url)
            : base("Resource not found: " + url) { }
    }
}