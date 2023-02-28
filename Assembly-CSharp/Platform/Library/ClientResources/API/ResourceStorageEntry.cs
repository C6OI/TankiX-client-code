using UnityEngine;

namespace Platform.Library.ClientResources.API {
    public class ResourceStorageEntry {
        public float LastAccessTime { get; set; }

        public Object Asset { get; set; }
    }
}