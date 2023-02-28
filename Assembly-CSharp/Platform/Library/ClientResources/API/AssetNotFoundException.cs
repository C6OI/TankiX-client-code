using System;

namespace Platform.Library.ClientResources.API {
    public class AssetNotFoundException : Exception {
        public AssetNotFoundException(string guid)
            : base("GUID=" + guid) { }
    }
}